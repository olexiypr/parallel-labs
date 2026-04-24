namespace Laba3.Task1;

public class Task1Tester
{
    public void TestDeadlock()
    {
        var examples = new Examples();
        examples.ExampleRaceCondition();
    }
}