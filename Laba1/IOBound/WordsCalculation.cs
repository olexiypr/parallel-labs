namespace Laba1.IOBound;

public class WordsCalculation(int threadsCount) : BaseTask(threadsCount)
{
    private readonly Random _random = new();
    private string _pathToFiles = "Files";
    private readonly char[] _letters = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    public long Result = 0;
    
    public void CalculateWordsCount(string pathToFiles)
    {
        var directories = Directory.GetDirectories(pathToFiles);
        Result += CalculateWordsCoundInDirectory(pathToFiles);
        var directoriesForThread = directories.Length / ThreadsCount;
        var restOfDirectories = directories.Length % ThreadsCount;
        using CountdownEvent countdown = new(ThreadsCount);
        for (int i = 0; i < ThreadsCount; i++)
        {
            var index = i;
            var thread = new Thread(() =>
            {
                var directoriesToCheck = directories
                    .Skip(index * directoriesForThread)
                    .Take(directoriesForThread);
                if (restOfDirectories > 0)
                {
                    directoriesToCheck = directoriesToCheck
                        .Append(directories[directoriesForThread * ThreadsCount + restOfDirectories - 1]);
                    restOfDirectories--;
                }

                //Console.WriteLine("Index: " + index + " " + string.Join(", ", directoriesToCheck));
                foreach (var directory in directoriesToCheck)
                { 
                    CalculateWordsCountSingle(directory);
                }
                countdown.Signal();
            });
            thread.Start();
        }
        
        countdown.Wait();

        Console.WriteLine(Result);
        /*foreach (var directory in directories)
        {
            //result += CalculateWordsCoundInDirectory(directory);
            CalculateWordsCount(directory);
        }*/
    }
    
    public void CalculateWordsCountSingle(string pathToFiles)
    {
        var directories = Directory.GetDirectories(pathToFiles);
        Interlocked.Add(ref Result, CalculateWordsCoundInDirectory(pathToFiles));
        foreach (var directory in directories)
        {
            //Result += CalculateWordsCoundInDirectory(directory);
            CalculateWordsCountSingle(directory);
        }
    }

    private int CalculateWordsCoundInDirectory(string pathToDirectory)
    {
        var files = Directory.GetFiles(pathToDirectory);
        var res = 0;
        foreach (var file in files)
        {
            var words = File.ReadAllText(file).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            res += words.Length;
        }
        
        return res;
    }
    
    public void GenerateFiles(string currentDirectory, int fileNumber, int minWordsNumber, int maxWordsNumber, int level = 1)
    {
        for (int i = 0; i < fileNumber; i++)
        {
            if (_random.Next(0, 100) < 30)
            {
                var path = $"{currentDirectory}/Subdir_{level}_{i}_Wave_1";
                Directory.CreateDirectory(path);
                GenerateFiles(path, fileNumber - fileNumber / 2 - 1, minWordsNumber, maxWordsNumber, level + 1);
            }
            
            var file = File.Open($"{currentDirectory}/File_{i}.txt", FileMode.OpenOrCreate);
            var writer = new StreamWriter(file);
            var wordsCount = _random.Next(minWordsNumber, maxWordsNumber);
            for (int j = 0; j < wordsCount; j++)
            {
                writer.Write(GetRandomWord(3, 20) + " ");
            }
            writer.Flush();
        }
    }

    private string GetRandomWord(int minWordLength, int maxWordLength)
    {
        var wordLength = _random.Next(minWordLength, maxWordLength);
        char[] word = new char[wordLength];
        for (int i = 0; i < wordLength; i++)
        {
            word[i] = _letters[_random.Next(_letters.Length)];
        }
        return new string(word);
    }
}