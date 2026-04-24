using System.Threading.Channels;

namespace Laba2.Task2;

public class ProducerConsumer
{
    public int ProducersCount { get; set; } = 10;
    public int ConsumersCount { get; set; } = 10;
    private const int CashbackPercent = 10;
    public int ItemsToProduce = 30;

    private Channel<Transaction> _channel = Channel.CreateUnbounded<Transaction>();

    private Dictionary<Guid, int> _users = Helper.GetUsers();
    
    private Producer _producer;

    public ProducerConsumer()
    {
        _producer = new Producer(ProducersCount, ItemsToProduce);
    }
    
    public void Start(CancellationTokenSource tokenSource)
    {
        new Thread(() =>
        {
            _producer.StartProducing(tokenSource.Token, transaction =>
            {
                _channel.Writer.TryWrite(transaction);
            }, () =>
            {
                //_channel.Writer.Complete();
                tokenSource.Cancel();
                Console.WriteLine("Producer finished");
                //Console.WriteLine(_channel.Reader.Completion.IsCompleted + " sdfsds");
            });
        }).Start();
        
        StartConsuming(tokenSource.Token);
        
        
        /*var random = new Random();
        using var countdown = new CountdownEvent(ProducersCount);
        for (int i = 0; i < ProducersCount; i++)
        {
            var thread = new Thread(() =>
            {
                while (!token.IsCancellationRequested && ProducedCount > 0)
                {
                    var transaction = new Transaction();
                    Interlocked.Decrement(ref ProducedCount);
                    //Console.WriteLine(ProducedCount);
                    _channel.Writer.TryWrite(transaction);
                    
                    Thread.Sleep(random.Next(1000));
                }  
                countdown.Signal();
                return;
            });
            thread.Start();
        }
        countdown.Wait(token);*/
    }

    public void StartConsuming(CancellationToken token)
    {
        var random = new Random();
        using var countdown = new CountdownEvent(ConsumersCount);
        for (int i = 0; i < ConsumersCount; i++)
        {
            var thread = new Thread(async () =>
            {
                while (!token.IsCancellationRequested || _channel.Reader.Count > 0)
                {
                    if (_channel.Reader.TryRead(out var transaction))
                    {
                        Console.WriteLine(transaction.Id + " " + transaction.UserId + " " + transaction.Amount + " " + transaction.Currency);
                        transaction.Amount = Helper.ConvertCurrency(transaction.Amount, transaction.Currency);
                        if (transaction.WithCashback)
                        {
                            ApplyCashback(transaction);
                        }
                        if (transaction.Type == TransactionType.Deposit)
                        {
                            ApplyDeposit(transaction);
                        }
                        else
                        {
                            ApplyWithdraw(transaction);
                        }
                        Thread.Sleep(random.Next(1000));
                    }
                }

                countdown.Signal();
            });
            thread.Start();
        }
        
        countdown.Wait();
    }
    
    private readonly Lock _lockObj = new();
    
    private void ApplyCashback(Transaction transaction)
    {
        transaction.Amount = transaction.Amount * (100 - CashbackPercent) / 100;
    }

    private void ApplyDeposit(Transaction transaction)
    {
        lock (_lockObj)
        {
            _users[transaction.UserId] += transaction.Amount;
        }
    }

    private void ApplyWithdraw(Transaction transaction)
    {
        lock (_lockObj)
        {
            _users[transaction.UserId] -= transaction.Amount;
        }
    }
}