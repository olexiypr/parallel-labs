namespace Laba1;

public class BaseTask(int threadsCount)
{
    protected readonly int ThreadsCount = threadsCount;
    protected void ThrowIfNotMultipleInput(int input)
    {
        if (input % ThreadsCount != 0)
        {
            throw new ArgumentException("Points count must be divisible by threads count");
        }   
    }
}