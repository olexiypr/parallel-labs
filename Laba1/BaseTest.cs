namespace Laba1;

public class BaseTest
{
    protected void TestAction(Action action)
    {
        var started = DateTime.Now.Ticks;
        Console.WriteLine("Started: " + started);
        action();
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);
    }
}