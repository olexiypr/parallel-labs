using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Laba2.Task1;

public class ArrayProcessingResult(double Median, double Min, double Max, decimal Avg)
{
    public double Median { get; set; } = Median;
    public double Min { get; set; } = Min;
    public double Max { get; set; } = Max;
    public decimal Avg { get; set; } = Avg;

    public void Deconstruct(out double Median, out double Min, out double Max, out decimal Avg)
    {
        Median = this.Median;
        Min = this.Min;
        Max = this.Max;
        Avg = this.Avg;
    }
    
    public override string ToString()
    {
        return $"Median: {Median} Min: {Min} Max: {Max} Avg: {Avg}";
    }
}

public class SequentialAlg
{
    public int[,] MultiplyMatrix(int[,] matrix1, int[,] matrix2)
    {
        var result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < matrix1.GetLength(1); k++)
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
            }
        }

        return result;
    }
    
    public ArrayProcessingResult ProcessArray(int[] array, bool sort = false)
    {
        var min = array[0];
        var max = array[0];
        decimal avg = 0;
        for (int i = 0; i < array.Length; i++)
        {
            avg += (decimal)array[i] / (decimal)array.Length;
            if (array[i] < min)
            {
                min = array[i];
            }
            if (array[i] > max)
            {
                max = array[i];
            }
        }

        double median = 0;
        if (sort)
        {
            SortArray(array);
            median = array.Length % 2 == 0 ? ((double)array[array.Length / 2 - 1] + (double)array[array.Length / 2]) / 2 : array[array.Length / 2];
        }
        
        var res = new ArrayProcessingResult(median, min, max, avg);
        //Console.WriteLine(res);
        return res;
    }

    public void SortArray(int[] array)
    {
        Array.Sort(array);
    }
    
    public void CalculateHtmlTagsFrequency()
    {
         var dictionary = new Dictionary<string, int>();
         var files = Directory.GetFiles(Helper.PathToHtmlFiles);
         foreach (var file in files)
         {
             CalculateHtmlTagsFrequencyInFile(dictionary, file);
         }
    }
    
    private void CalculateHtmlTagsFrequencyInFile(Dictionary<string, int> dictionary, string file)
    {
        var html = File.ReadAllText(file);
        var regex = new Regex("<[^>]+>");
        var words = regex.Matches(html);
        foreach (var word in words.Select(m => m.Value))
        {
            if (!dictionary.TryAdd(word, 1))
            {
                dictionary[word]++;
            }
        }
    }

    private readonly Lock lockObj = new();
    
    public void CalculateHtmlTagsFrequencyInFileConcurrent(Dictionary<string, int> dictionary, string file)
    {
        var html = File.ReadAllText(file); 
        var regex = new Regex("<[^>]+>");
        var words = regex.Matches(html);
        foreach (var word in words.Select(m => m.Value))
        {
            lock (lockObj)
            {
                if (!dictionary.TryAdd(word, 1))
                {
                    dictionary[word]++;
                }
            }
        }
    }
}