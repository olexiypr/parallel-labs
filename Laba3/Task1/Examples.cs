namespace Laba3.Task1;

public class Examples
{
    private Lock _lock1 = new();
    private Lock _lock2 = new();
    private int _counter = 0;

    public void ExampleRaceCondition()
    {
        var t1 = new Thread(IncrementCounter);
        var t2 = new Thread(IncrementCounter);
        var t3 = new Thread(IncrementCounter);
        t1.Start();
        t2.Start();
        t3.Start();
        t1.Join();
        t2.Join();
        t3.Join();
        Console.WriteLine(_counter);
    }

    private void IncrementCounter()
    {
        for (int i = 0; i < 10000; i++)
        {
            _counter++;
        }
    }
    
    public void Example2LocksDeadlock()
    {
        var t1 = new Thread(Thread1Method);
        var t2 = new Thread(Thread2Method);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }
    
    private void Thread1Method()
    {
        lock (_lock1)
        {
            Console.WriteLine("Thread 1 acquired Lock1.");
            Thread.Sleep(100);

            lock (_lock2)
            {
                Console.WriteLine("Thread 1 acquired both locks.");
            }
        }
    }

    private void Thread2Method()
    {
        lock (_lock2)
        {
            Console.WriteLine("Thread 2 acquired Lock2.");
            Thread.Sleep(100);

            lock (_lock1)
            {
                Console.WriteLine("Thread 2 acquired both locks.");
            }
        }
    }
}