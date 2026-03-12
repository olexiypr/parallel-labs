using System.Numerics;

namespace Laba1.CPUBound;

public class CPUBound : BaseTest
{

    public void TestMonteKarlo()
    {
        var monteKarlo = new MonteKarlo(2);
        var started = DateTime.Now.Ticks;
        Console.WriteLine("Started: " + started);
        monteKarlo.CalculatePi(100000000);
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);
    }
    
    public void TestFactorization()
    {
        var factorization = new Factorization(4);
        //10000000000001107
        var input = BigInteger.Parse("100000000000000132342");
        //TestAction(() => factorization.CalculateSingleThread(input));
        TestAction(() => factorization.Calculate(input).GetAwaiter().GetResult());
    }

    public void TestSimpleNumbersCalculation()
    {
        var simpleNumbersCalculation = new SimpleNumbersCalculation(5);
        TestAction(() => simpleNumbersCalculation.Calculate(1000000));
    }
}