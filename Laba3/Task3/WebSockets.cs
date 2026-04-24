namespace Laba3.Task3;

using System.Net;
using System.Net.WebSockets;
using System.Text;

public class WebSockets
{
    public async Task StartGeneratingNumbers(int messagesCount = 10000)
    {
        const string wsUri = "ws://localhost:8765/ws/";
        using var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(wsUri), CancellationToken.None);

        var rng = new Random();
        var receiveBuffer = new byte[1024];
        var started = DateTime.Now.Ticks;
        Console.WriteLine("Started: " + started);
        for (var i = 0; i < messagesCount; i++)
        {
            var sentNumber = rng.Next(0, 10000);
            var text = sentNumber.ToString();
            var payload = Encoding.UTF8.GetBytes(text);

            await client.SendAsync(
                new ArraySegment<byte>(payload),
                WebSocketMessageType.Text,
                endOfMessage: true, CancellationToken.None);

            var receive = await client.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var echoedText = Encoding.UTF8.GetString(receiveBuffer, 0, receive.Count);

            if (!int.TryParse(echoedText, out var receivedNumber))
            {
                throw new InvalidOperationException($"Received non-numeric payload: '{echoedText}'");
            }

            var isMatch = sentNumber == receivedNumber;

            if (!isMatch)
            {
                throw new InvalidOperationException($"Mismatch. Sent={sentNumber}, Received={receivedNumber}");
            }
        }
        
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);

        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
    }
}