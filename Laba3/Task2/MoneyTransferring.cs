namespace Laba3.Task2;

public class MoneyTransferring
{
    private ConcurrentAccounts _accounts = new();
    
    public void StartTransferring(int threadsCount = 1000, int accountsCount = 100)
    {
        for (int i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(() =>
            {
                var random = new Random();
                _accounts.Transfer(random.Next(0, accountsCount), random.Next(0, accountsCount), 100);
            });
            thread.Start();
        }
    }
}