namespace Laba3.Task2;

using System;
using System.Collections.Generic;

public class ConcurrentAccounts
{
    private readonly object _sync = new();
    private readonly List<Accounts.Account> _accounts = new();

    public ConcurrentAccounts()
    {
        for (int i = 0; i < 100; i++)
        {
            var userId = i + 1;
            var balance = 1000 + (i * 100);
            _accounts.Add(new Accounts.Account(userId, balance));
        }
    }

    public bool Transfer(int fromUserId, int toUserId, int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Transfer amount must be positive.");
        }

        if (fromUserId == toUserId)
        {
            return true;
        }

        var from = FindAccount(fromUserId);
        var to = FindAccount(toUserId);

        if (from is null || to is null)
        {
            return false;
        }

        lock (from)
        {
            Thread.Sleep(100);
            if (from.Balance < amount)
            {
                return false;
            }
            from.Balance -= amount;

            Console.WriteLine("Transfer from {0} to {1} amount {2}", fromUserId, toUserId, amount);
            
            lock (to)
            {
                to.Balance += amount;
            }
        }
        return true;
    }

    private Accounts.Account? FindAccount(int userId)
    {
        return _accounts.Find(a => a.UserId == userId);
    }
}