using System.Numerics;
using System.Threading.Channels;

namespace Laba1.CPUBound;

public class Factorization(int threadsCount) : BaseTask(threadsCount)
{
    private readonly Channel<int> _channel = Channel.CreateUnbounded<int>();
    private readonly CancellationTokenSource _cts = new();
    private bool[] _isFound = new bool [threadsCount];
    public async Task Calculate(BigInteger input)
    {
        var curr = input;
        int currSimpleNumber = 2;
        var res = "";
        StartCalculatingSimpleNumbers(_cts.Token);
        while (!IsSimple(curr))
        {
            while (curr % currSimpleNumber != 0)
            {
                //currSimpleNumber = GetNextSimpleNumber(currSimpleNumber);
                currSimpleNumber = await _channel.Reader.ReadAsync();
            }
            
            for (int i = 0; i < ThreadsCount; i++)
            {
                _isFound[i] = true;
            }
            curr /= currSimpleNumber;
            Console.Write(currSimpleNumber + " * ");
            res += currSimpleNumber + " * ";
            currSimpleNumber = 2;
        }
        await _cts.CancelAsync();
        Console.WriteLine();

        Console.WriteLine(res + curr);
        _cts.Dispose();
    }

    private void StartCalculatingSimpleNumbers(CancellationToken cancellationToken)
    {
        for (int i = 1; i <= ThreadsCount; i++)
        {
            var index = i;
            var thread = new Thread(() =>
            {
                var ind = index;
                var id = int.Parse(Thread.CurrentThread.Name);
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_isFound[id])
                    {
                        ind = index;
                        _isFound[id] = false;
                    }
                    if (ind >= 2 && IsSimple(ind))
                    {
                        _channel.Writer.TryWrite(ind);
                    }
                    ind += ThreadsCount;
                }
            });
            thread.Name = (i - 1).ToString();
            thread.Start();
        }
    }

    private void StartCheckingNumberSimplicity(CancellationToken cancellationToken)
    {
        for (int i = 0; i < ThreadsCount; i++)
        {
            var thread = new Thread(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                }
            });
        }
    }

    public void CalculateSingleThread(BigInteger input)
    {
        var curr = input;
        int currSimpleNumber = 2;
        var res = "";
        while (!IsSimple(curr))
        {
            while (curr % currSimpleNumber != 0)
            {
                currSimpleNumber = GetNextSimpleNumber(currSimpleNumber);
            }
            
            curr /= currSimpleNumber;
            Console.Write(currSimpleNumber + " * ");
            res += currSimpleNumber + " * ";
            currSimpleNumber = 2;
        }

        Console.WriteLine();

        Console.WriteLine(res + curr);
    }
    
    private int GetNextSimpleNumber(int number)
    {
        number++;
        while (!IsSimple(number)) number++;
        return number;
    }
    
    private bool IsSimple(BigInteger number)
    {
        for (int i = 2; i < number / 2; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        
        return true;
    }
}