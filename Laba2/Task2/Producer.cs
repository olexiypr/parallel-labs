namespace Laba2.Task2;

public class Producer
{
    private int _producersCount;
    private int _itemsToProduce;
    
    public Producer(int producersCount, int itemsToProduce = 1000)
    {
        _producersCount = producersCount;
        _itemsToProduce = itemsToProduce;
    }
    
    public void StartProducing(CancellationToken token, Action<Transaction> transactionCreatedCallback, Action onFinishCallback)
    {
        var random = new Random();
        using var countdown = new CountdownEvent(_producersCount);
        for (int i = 0; i < _producersCount; i++)
        {
            var thread = new Thread(() =>
            {
                while (!token.IsCancellationRequested && _itemsToProduce > 0)
                {
                    var transaction = new Transaction();
                    Interlocked.Decrement(ref _itemsToProduce);
                    Console.WriteLine(_itemsToProduce);
                    transactionCreatedCallback(transaction);
                    //_channel.Writer.TryWrite(transaction);
                    Thread.Sleep(random.Next(1000));
                }  
                countdown.Signal();
            });
            thread.Start();
        }
        countdown.Wait();
        onFinishCallback();
    }
}