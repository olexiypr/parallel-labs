using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Laba1.CPUBound;

public class MonteKarlo(int threadsCount) : BaseTask(threadsCount)
{
    private const int BatchSize = 100;
    private readonly Random _random = new Random();
    private readonly ConcurrentQueue<string> _batchQueue = new();

    public double CalculatePi(int input)
    {
        ThrowIfNotMultipleInput(input);
        int[] result = new int[ThreadsCount];
        using CountdownEvent countdown = new(ThreadsCount);
        for (var index = 0; index < ThreadsCount; index++)
        {
            var currIndex = index;
            var thread = new Thread(() =>
            {
                try
                {
                    int dotsInCircle = 0;
                    var random = new Random();
                    for (int i = 0; i < input / ThreadsCount; i++)
                    {
                        var point = GenerateRandomPoint(random);

                        if (IsInCircle(point))
                        {
                            dotsInCircle++;
                        }
                    }

                    result[currIndex] = dotsInCircle;
                }
                finally
                {
                    countdown.Signal();
                }
            });
            thread.Start();
        }

        countdown.Wait();
        
        double sum = 0;
        for (int i = 0; i < ThreadsCount; i++)
        {
            sum += result[i] / (double)input;
        }
        return sum * 4;
    }
    
    private (double, double) GenerateRandomPoint(Random random)
    {
        var x = random.NextDouble();
        var y = random.NextDouble();
        return (x, y);
    }

    private static bool IsInCircle((double, double) point)
    {
        var (x, y) = point;
        return Math.Sqrt(x * x + y * y) < 1;
    }

    public async Task CalculatePiAndStreamAsync(int input, CancellationToken cancellationToken = default)
    {
        using var ws = new ClientWebSocket();
        await ws.ConnectAsync(new Uri("ws://localhost:8765"), cancellationToken);

        var done = false;

        _ = Task.Run(async () =>
        {
            while (!done || !_batchQueue.IsEmpty)
            {
                if (_batchQueue.TryDequeue(out var json))
                {
                    var bytes = Encoding.UTF8.GetBytes(json);
                    await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true,
                        cancellationToken);
                }
                else
                {
                    await Task.Delay(10, cancellationToken);
                }
            }
        }, cancellationToken);

        double pi = CalculatePi(input);
        done = true;

        // Drain remaining batches
        while (_batchQueue.TryDequeue(out var json))
        {
            Console.WriteLine(json);
            var bytes = Encoding.UTF8.GetBytes(json);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }

        // Send final result
        var resultJson = JsonSerializer.Serialize(new { type = "result", pi });
        var resultBytes = Encoding.UTF8.GetBytes(resultJson);
        await ws.SendAsync(new ArraySegment<byte>(resultBytes), WebSocketMessageType.Text, true, cancellationToken);

        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", cancellationToken);
    }

    private void EnqueueBatch(List<(double x, double y)> batch)
    {
        var payload = new
        {
            type = "batch",
            points = batch.Select(p => new { p.x, p.y }).ToArray()
        };
        _batchQueue.Enqueue(JsonSerializer.Serialize(payload));
    }
}