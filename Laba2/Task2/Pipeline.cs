using System.Threading.Tasks.Dataflow;

namespace Laba2.Task2;

public class Pipeline
{
    public int ConsumersCount { get; set; } = 10;
    private Dictionary<Guid, int> _users = Helper.GetUsers();
    private const int CashbackPercent = 10;
    private readonly Lock _lockObj = new();
    private Producer _producer;

    public Pipeline()
    {
        _producer = new Producer(10, 30);
    }

    public void Start(CancellationTokenSource tokenSource)
    {
        var uploadBlock = new TransformBlock<Transaction, Transaction>(transaction => transaction);

        var applyCashback = new TransformBlock<Transaction, Transaction>(transaction =>
        {
            Console.WriteLine("Id: " + transaction.Id + " " + transaction.UserId + " " + transaction.Amount + " " + transaction.Currency + "");
            transaction.Amount = transaction.Amount * (100 - CashbackPercent) / 100;
            return transaction;
        });
        
        var applyTransaction = new ActionBlock<Transaction>(transaction =>
        {
            if (transaction.Type == TransactionType.Deposit)
            {
                lock (_lockObj)
                {
                    _users[transaction.UserId] += transaction.Amount;
                }
            }
            else
            {
                lock (_lockObj)
                {
                    _users[transaction.UserId] -= transaction.Amount;
                }
            }
        });

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        uploadBlock.LinkTo(applyCashback, linkOptions);
        applyCashback.LinkTo(applyTransaction, linkOptions);
        
        new Thread(() =>
        {
            _producer.StartProducing(tokenSource.Token, transaction =>
            {
                uploadBlock.Post(transaction);
            }, () =>
            {
                uploadBlock.Complete();
                uploadBlock.Completion.Wait();
                tokenSource.Cancel();
                Console.WriteLine("Producer finished");
            });
        }).Start();
    }
}