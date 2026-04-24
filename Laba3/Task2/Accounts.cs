namespace Laba3.Task2;

using System;
using System.Collections.Generic;

public class Accounts
{
    private readonly List<Account> _accounts = new();

    public class Account
    {
        public int UserId { get; }
        public int Balance { get; set; }

        public Account(int userId, int balance)
        {
            if (balance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(balance), "Balance cannot be negative.");
            }

            UserId = userId;
            Balance = balance;
        }
    }

    public Accounts()
    {
        for (int i = 0; i < 100; i++)
        {
            var userId = i + 1;
            var balance = 1000 + (i * 100);
            _accounts.Add(new Account(userId, balance));
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

        var from = FindAccountUnsafe(fromUserId);
        var to = FindAccountUnsafe(toUserId);

        if (from is null || to is null)
        {
            return false;
        }

        if (from.Balance < amount)
        {
            return false;
        }

        from.Balance -= amount;
        to.Balance += amount;
        return true;
    }

    private Account? FindAccountUnsafe(int userId)
    {
        return _accounts.Find(a => a.UserId == userId);
    }
}