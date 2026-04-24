using System.Collections.Concurrent;

namespace Laba2.Task1;

public class ForkJoin
{
    private int _treshold = 10;
    private int _arrTreshold = 100000;
    private int _matrixTreshold = 10;
    private readonly SequentialAlg _sequentialAlg = new();
    
    public int[,] MultiplyMatrix(int[,] matrix1, int[,] matrix2, int threadsCount = 10)
    {
        var result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
        var results = new LinkedList<MatrixResult>();
        ProcessMatrixRecursive(results, matrix1, matrix2, threadsCount);
        var resultArr = results.OrderBy(r => r.BeginIndex).ToArray();
        for (int i = 0; i < results.Count; i++)
        {
            for (int j = i * result.GetLength(0) / resultArr.Length; j < i * result.GetLength(0) / resultArr.Length + result.GetLength(0) / resultArr.Length; j++)
            {
                for (int k = 0; k < result.GetLength(1); k++)
                {
                    result[j, k] = resultArr[i].Matrix[j - i * result.GetLength(0) / resultArr.Length, k];
                }
            }
        }
        return result;
    }

    private void ProcessMatrixRecursive(LinkedList<MatrixResult> results, int[,] matrix1, int[,] matrix2, int threadsCount, int beginIndex = 0, int endIndex = 0)
    {
        if (matrix1.GetLength(0) <= _matrixTreshold)
        {
            results.AddLast(new MatrixResult(_sequentialAlg.MultiplyMatrix(matrix1, matrix2), beginIndex, endIndex));
            return;
        }
        var tasks = new List<Task>();
        for (int i = 0; i < threadsCount; i++)
        {
            int index = i;
            var task = Task.Run(() =>
            {
                var subMatr = new int[matrix1.GetLength(0) / threadsCount, matrix1.GetLength(1)];
                for (int s = 0; s < subMatr.GetLength(0); s++)
                {
                    for (int k = 0; k < subMatr.GetLength(1); k++)
                    {
                        subMatr[s, k] = matrix1[index * matrix1.GetLength(0) / threadsCount + s, k];
                    }
                }
                ProcessMatrixRecursive(results, subMatr, matrix2, threadsCount, beginIndex + index * matrix1.GetLength(0) / threadsCount,  beginIndex + (index + 1) * matrix1.GetLength(0) / threadsCount);
            });
            tasks.Add(task);
        }
        Task.WhenAll(tasks).GetAwaiter().GetResult();
    }

    public void ProcessArray(int[] array, int threadsCount = 10)
    {
        var results = new LinkedList<ArrayProcessingResult>();
        ProcessArrayRecursive(results, array, threadsCount);
        var arrayProcessingResult = new ArrayProcessingResult(Min: results.Select(r => r.Min).Min(),
            Max: results.Select(r => r.Max).Max(), Avg: results.Select(r => r.Avg).Average(), Median: 0);

        Console.WriteLine(arrayProcessingResult);

        var sortedArr = array
            .AsParallel()
            .WithDegreeOfParallelism(threadsCount)
            .OrderBy(x => x)
            .ToArray();

        double median = sortedArr.Length % 2 == 0 ? ((double)sortedArr[sortedArr.Length / 2 - 1] + (double)sortedArr[sortedArr.Length / 2]) / 2 : sortedArr[sortedArr.Length / 2];

        Console.WriteLine("Median: " + median);

        //Console.WriteLine(result);
    }

    private void ProcessArrayRecursive(LinkedList<ArrayProcessingResult> results, int[] array, int threadsCount)
    {
        if (array.Length <= _arrTreshold)
        {
            Array.Sort(array);
            var result = _sequentialAlg.ProcessArray(array);
            results.AddLast(result);
            return;
        }
        var tasks = new List<Task>(threadsCount);
        for (int i = 0; i < threadsCount; i++)
        {
            var index = i;
            var task = Task.Run(() =>
            {
                var subArr = array
                    .Skip(index * array.Length / threadsCount)
                    .Take(array.Length / threadsCount)
                    .ToArray();
                
                ProcessArrayRecursive(results, subArr, threadsCount);
            });
            tasks.Add(task);
        }
        
        Task.WhenAll(tasks).GetAwaiter().GetResult();
    }
    
    public void CalculateHtmlTagsFrequency(int tasksPerIteration = 10)
    {
        var dictionary = new Dictionary<string, int>();
        var files = Directory.GetFiles(Helper.PathToHtmlFiles);
        CalculateHtmlTagsFrequency(dictionary, files, tasksPerIteration);
    }

    private void CalculateHtmlTagsFrequency(Dictionary<string, int> dictionary, string[] files, int tasksPerIteration)
    {
        if (files.Length <= _treshold)
        {
            foreach (var file in files)
            {
                _sequentialAlg.CalculateHtmlTagsFrequencyInFileConcurrent(dictionary, file);
            }
            return;
        }
        var tasks = new List<Task>(files.Length / tasksPerIteration);
        for (int i = 0; i < tasksPerIteration; i++)
        {
            var index = i;
            var task = Task.Run(() =>
            {
                var filesToProcess = files.Skip(index * files.Length / tasksPerIteration).Take(files.Length / tasksPerIteration);
                CalculateHtmlTagsFrequency(dictionary, filesToProcess.ToArray(), tasksPerIteration);
            });
            tasks.Add(task);
        }
        Task.WhenAll(tasks).GetAwaiter().GetResult();
    }
}