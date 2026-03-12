namespace Laba1.IOBound;

public class IOBound : BaseTest
{
    public void TestWordsCalculation()
    {
        /*if (!Directory.Exists("Files"))
        {
            Directory.CreateDirectory("Files");
        }*/
        /*var wordsCalculationn = new WordsCalculation(1);
        var dirs = Directory.GetDirectories("Files");
        foreach (var dir in dirs)
        { 
            wordsCalculationn.GenerateFiles(dir,50, 10, 40);
        }*/
        var wordsCalculationSinge = new WordsCalculation(1);
        var wordsCalculation = new WordsCalculation(10);
        long resultSinge = 0;
        long resultMulti = 0;
        /*TestAction(() => wordsCalculationSinge.CalculateWordsCountSingle("Files"));
        Console.WriteLine(wordsCalculationSinge.Result);*/
        
        TestAction(() => wordsCalculation.CalculateWordsCount("Files"));
        Console.WriteLine(wordsCalculation.Result);
    }
}