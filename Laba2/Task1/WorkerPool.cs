using System.Collections.Concurrent;

namespace Laba2.Task1;

public class WorkerPool
{
    public int[,] MultiplyMatrix(int[,] matrix1, int[,] matrix2, int workersCount = 10, int tasksCount = 10)
    {
        var result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
        using var countdown = new CountdownEvent(tasksCount);
        ThreadPool.SetMaxThreads(workersCount, 2);
        for (int i = 0; i < tasksCount; i++)
        {
            var index = i;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int j = index * result.GetLength(0) / tasksCount; j < index * result.GetLength(0) / tasksCount + result.GetLength(0) / tasksCount; j++)
                {
                    for (int f = 0; f < result.GetLength(1); f++)
                    {
                        result[j, f] = 0;
                        for (int k = 0; k < matrix1.GetLength(1); k++)
                            result[j, f] += matrix1[j, k] * matrix2[k, f];
                    }
                }
                countdown.Signal();
            });
        }
        
        countdown.Wait();

        return result;
    }
    
    public void ProcessArray(int[] array, int workersCount = 10)
    {
        var sequentialAlg = new SequentialAlg();
        var results = new List<ArrayProcessingResult>(workersCount);
        using var countdown = new CountdownEvent(workersCount);
        ThreadPool.SetMaxThreads(workersCount, 2);
        for (int i = 0; i < workersCount; i++)
        {
            var index = i;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                var subArr = array
                    .Skip(index * array.Length / workersCount)
                    .Take(array.Length / workersCount)
                    .ToArray();
                var res = sequentialAlg.ProcessArray(subArr);
                results.Add(res);
                countdown.Signal();
            });
        }
        countdown.Wait();
        var result = results.Aggregate(seed: new ArrayProcessingResult(0, int.MaxValue, 0, 0), (curr, next) =>
        {
            var min = Math.Min(curr.Min, next.Min);
            var max = Math.Max(curr.Max, next.Max);
            curr.Avg += next.Avg / workersCount;
            curr.Min = min;
            curr.Max = max;
            return curr;
        });
        //var sortedArr = new int[array.Length];
        using var countdownEvent = new CountdownEvent(1);
        ThreadPool.QueueUserWorkItem(_ =>
        {
            Array.Sort(array);
            countdownEvent.Signal();
        });
        
        countdownEvent.Wait();
        
        double median = array.Length % 2 == 0 ? ((double)array[array.Length / 2 - 1] + (double)array[array.Length / 2]) / 2 : array[array.Length / 2];

        Console.WriteLine("Median: " + median);

        Console.WriteLine(result);
    }
    
    
    public void CalculateHtmlTagsFrequency(int workersCount = 10)
    {
        ThreadPool.SetMaxThreads(workersCount, 2);
        var dictionary = new Dictionary<string, int>();
        var files = Directory.GetFiles(Helper.PathToHtmlFiles);
        using var countdown = new CountdownEvent(files.Length);
        var singleThreadAlg = new SequentialAlg();
        foreach (var file in files)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                singleThreadAlg.CalculateHtmlTagsFrequencyInFileConcurrent(dictionary, file);
                countdown.Signal();   
            });
        }
        countdown.Wait();
    }

    private readonly ConcurrentQueue<string> _queue = new();

    public void CalculateHtmlTagsFrequencyCustomPool(int workersCount = 10)
    {
        var dictionary = new Dictionary<string, int>();
        var files = Directory.GetFiles(Helper.PathToHtmlFiles);
        using var cancellationToken = new CancellationTokenSource();
        var singleThreadAlg = new SequentialAlg();
        for (var index = 0; index < workersCount; index++)
        {
            var thread = new Thread(() =>
            {
                RunTaskProcessing(cancellationToken.Token, s =>
                {
                    singleThreadAlg.CalculateHtmlTagsFrequencyInFileConcurrent(dictionary, s);
                });
            });
            thread.Start();
        }

        foreach (var file in files)
        {
            _queue.Enqueue(file);
        }

        while (_queue.Count > 0)
        {
            
        }

        if (_queue.IsEmpty)
        {
            cancellationToken.Cancel();
        }
    }

    private void RunTaskProcessing(CancellationToken cancellationToken, Action<string> action)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var item))
            {
                action(item);
            }
        }
    }
}