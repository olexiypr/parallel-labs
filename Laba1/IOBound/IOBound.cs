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
        var name = "WordsCalculation";
        LogBeginTest(name, "");
        var wordsCalculationSinge = new WordsCalculation(1);
        TestAction(() => wordsCalculationSinge.CalculateWordsCountSingle("Files"), name, 1);
        LogCompletedTest(name);
        
        LogBeginTest(name, "");
        for (int i = 1; i <= 21; i+=4)
        {
            var wordsCalculation = new WordsCalculation(i);
            TestAction(() => wordsCalculation.CalculateWordsCount("Files"), name, i);
        }
        
        var wordsCalculation1 = new WordsCalculation(30);
        TestAction(() => wordsCalculation1.CalculateWordsCount("Files"), name, 30);
        
        var wordsCalculation2 = new WordsCalculation(40);
        TestAction(() => wordsCalculation2.CalculateWordsCount("Files"), name, 40);
        
        long resultSinge = 0;
        long resultMulti = 0;
    }
}