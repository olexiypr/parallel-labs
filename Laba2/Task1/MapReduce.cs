using System.Text.RegularExpressions;

namespace Laba2.Task1;

public class MapReduce
{
    public int[,] MultiplyMatrix(int[,] matrix1, int[,] matrix2, int threadsCount = 10)
    {
        var result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
        var tasks = new List<Task<int[,]>>(threadsCount);
        var sequentialAlg = new SequentialAlg();
        for (int i = 0; i < threadsCount; i++)
        {
            int index = i;
            var task = Task.Run(() =>
            {
                var subMatr = new int[result.GetLength(0) / threadsCount, matrix1.GetLength(1)];
                for (int s = 0; s < subMatr.GetLength(0); s++)
                {
                    for (int k = 0; k < subMatr.GetLength(1); k++)
                    {
                        subMatr[s, k] = matrix1[index * result.GetLength(0) / threadsCount + s, k];
                    }
                }
                return sequentialAlg.MultiplyMatrix(subMatr, matrix2);
            });
            tasks.Add(task);
        }
        Task.WhenAll(tasks).GetAwaiter().GetResult();
        var results = tasks.Select(t => t.Result).ToArray();
        for (int i = 0; i < results.Length; i++)
        {
            for (int j = i * result.GetLength(0) / threadsCount; j < i * result.GetLength(0) / threadsCount + result.GetLength(0) / threadsCount; j++)
            {
                for (int k = 0; k < result.GetLength(1); k++)
                {
                    result[j, k] = results[i][j - i * result.GetLength(0) / threadsCount, k];
                }
            }
        }
        return result;
    }
    
    
    public void ProcessArray(int[] array, int threadsCount = 10)
    {
        var tasks = new List<Task<ArrayProcessingResult>>(threadsCount);
        var sequentialAlg = new SequentialAlg();
        for (int i = 0; i < threadsCount; i++)
        {
            var index = i;
            var task = Task.Run(() =>
            {
                var subArr = array
                    .Skip(index * array.Length / threadsCount)
                    .Take(array.Length / threadsCount)
                    .ToArray();
                return sequentialAlg.ProcessArray(subArr);
            });
            tasks.Add(task);
        }
        Task.WhenAll(tasks).GetAwaiter().GetResult();
        var result = tasks.Select(t => t.Result).Aggregate(seed: new ArrayProcessingResult(0, int.MaxValue, 0, 0), (curr, next) =>
        {
            var min = Math.Min(curr.Min, next.Min);
            var max = Math.Max(curr.Max, next.Max);
            curr.Avg += next.Avg / threadsCount;
            curr.Min = min;
            curr.Max = max;
            return curr;
        });

        var sortedArr = array
            .AsParallel()
            .WithDegreeOfParallelism(threadsCount)
            .OrderBy(x => x)
            .ToArray();
        
        double median = sortedArr.Length % 2 == 0 ? ((double)sortedArr[sortedArr.Length / 2 - 1] + (double)sortedArr[sortedArr.Length / 2]) / 2 : sortedArr[sortedArr.Length / 2];

        Console.WriteLine("Median: " + median);

        Console.WriteLine(result);
    }
    
    
    public void CalculateHtmlTagsFrequency(int threadsCount = 10)
    {
        var dictionary = new Dictionary<string, int>();
        var files = Directory.GetFiles(Helper.PathToHtmlFiles);
        var res = files.AsParallel().WithDegreeOfParallelism(threadsCount).Select(file =>
        {
            var html = File.ReadAllText(file);
            var regex = new Regex("<[^>]+>");
            var words = regex.Matches(html);
            var dict = new Dictionary<string, int>();
            foreach (var word in words.Select(m => m.Value))
            {
                if (!dict.TryAdd(word, 1))
                {
                    dict[word]++;
                }
            }

            return dict;
        }).Aggregate(dictionary, (current, next) =>
        {
            foreach (var pair in next.Where(pair => !current.TryAdd(pair.Key, pair.Value)))
            {
                current[pair.Key] += pair.Value;
            }

            return current;
        });
    }
}