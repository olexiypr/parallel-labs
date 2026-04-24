namespace Laba1;

public class BaseTest
{
    public const string ResultPath = "Results.txt";
    protected void TestAction(Action action, string name, int threadsCount)
    {
        var started = DateTime.Now.Ticks;
        WriteToResultFile("Started: " + started);
        action();
        var finished = DateTime.Now.Ticks;
        WriteToResultFile("Finished: " + finished);
        WriteToResultFile("Time: " + (finished - started) / 10000);
        LogTestResult(name, threadsCount, finished - started);
    }
    
    protected void LogBeginTest<T>(string name, T param)
    {
        WriteToResultFile(new string('-', 20));
        WriteToResultFile($"{name} testing begin with param: {param}");
        WriteToResultFile(new string('-', 20));
    }

    protected void LogCompletedTest(string name)
    {
        WriteToResultFile($"{name} completed");
    }

    private void LogTestResult(string name, int threadsCount, long time)
    {
        using var file = new StreamWriter(ResultPath, true);
        file.WriteLine($"{DateTime.Now.ToString()} | Task: {name} | Threads Number: {threadsCount} | Time: {time}");
    }

    private void WriteToResultFile(string line)
    {
        using var file = new StreamWriter(ResultPath, true);
        file.WriteLine(line);
        file.Flush();
        WriteToDebug(line);
    }
    
    private void WriteToDebug(string line)
    {
        Console.WriteLine(line);
    }
}