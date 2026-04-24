namespace Laba2.Task2;

public class Task2Tester
{
    public void TestProducerConsumer()
    {
        var producerConsumer = new ProducerConsumer();
        using var cancellationTokenSource = new CancellationTokenSource();
        //producerConsumer.Start(cancellationTokenSource);
        
        var pipelined = new Pipeline();
        pipelined.Start(cancellationTokenSource);
    }
}