namespace Laba1.CPUBound;

public class SimpleNumbersCalculation(int threadsCount) : BaseTask(threadsCount)
{
    public void Calculate(int input)
    {
        ThrowIfNotMultipleInput(input);
        using CountdownEvent countdown = new(ThreadsCount);
        var partitionSize = input / ThreadsCount;
        for (int i = 0; i < ThreadsCount; i++)
        {
            var index = i;
            var thread = new Thread(() =>
            {
                try
                {
                    ProcessPartition(index * partitionSize, partitionSize * (index + 1));
                }
                finally
                {
                    countdown.Signal();
                }
            });
            thread.Start();
        }
        countdown.Wait();
    }
    
    private void ProcessPartition(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            if (IsSimple(i))
            {
                //Console.WriteLine(i);
            }
        }
    }

    private bool IsSimple(int number)
    {
        if (number <= 1)
        {
            return false;
        }
        for (int i = 2; i <= number / 2; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        
        return true;
    }
}