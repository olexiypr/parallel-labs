using System;
using System.IO;
using System.Text;
using Laba2.Task2;

namespace Laba2;

public class Helper
{
    public static Dictionary<Guid, int> GetUsers()
    {
        return new Dictionary<Guid, int>
        {
            { Guid.Parse("d2f7c3a9-8e4b-4c12-9a2f-0b1c2d3e4f50"), 1000 },
            { Guid.Parse("1a2b3c4d-5e6f-7081-92a3-b4c5d6e7f809"), 1000 },
            { Guid.Parse("aa11bb22-cc33-dd44-ee55-001122334455"), 1000 },
            { Guid.Parse("0f1e2d3c-4b5a-6978-8c9d-0a1b2c3d4e5f"), 1000 },
            { Guid.Parse("12345678-90ab-cdef-1234-567890abcdef"), 1000 },
            { Guid.Parse("fedcba98-7654-3210-fedc-ba9876543210"), 1000 },
            { Guid.Parse("abcdef01-2345-6789-abcd-ef0123456789"), 1000 },
            { Guid.Parse("11111111-2222-3333-4444-555555555555"), 1000 },
            { Guid.Parse("99999999-8888-7777-6666-555555555555"), 1000 },
            { Guid.Parse("0a0b0c0d-0e0f-1011-1213-141516171819"), 1000 },
            { Guid.Parse("cafebabe-dead-beef-cafe-babecafedead"), 1000 },
            { Guid.Parse("7f6e5d4c-3b2a-1908-0706-050403020100"), 1000 },
            { Guid.Parse("abcdefab-cdef-abcd-efab-cdefabcdefab"), 1000 },
            { Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), 1000 },
            { Guid.Parse("89abcdef-0123-4567-89ab-cdef01234567"), 1000 },
            { Guid.Parse("deadbeef-dead-beef-dead-beefdeadbeef"), 1000 },
            { Guid.Parse("00112233-4455-6677-8899-aabbccddeeff"), 1000 },
            { Guid.Parse("10203040-5060-7080-90a0-b0c0d0e0f000"), 1000 },
            { Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 1000 },
            { Guid.Parse("123e4567-e89b-12d3-a456-426614174000"), 1000 }
        };
    }
    
    public static int ConvertCurrency(int amount, Currency fromCurrency, Currency toCurrency = Currency.UAH)
    {
        if (fromCurrency == toCurrency) return amount;
        if (fromCurrency == Currency.USD) return amount * 43;
        if (fromCurrency == Currency.EUR) return amount * 51;
        throw new ArgumentException("Invalid currency");
    } 
    public static int[,] GenerateRandomMatrix(int sizeX, int sizeY)
    {
        var random = new Random();
        var result = new int[sizeX, sizeY];
        for (int i = 0; i < sizeX ; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                result[i, j] = random.Next(0, 1000);
            }
        }
        return result;
    }

    public static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static bool ValidateMatrixMultiply(int[,] matrix1, int[,] matrix2)
    {
        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix1.GetLength(1); j++)
            {
                if (matrix1[i, j] != matrix2[i, j])
                {
                    Console.WriteLine($"Error: matrix1[{i}, {j}] != matrix2[{i}, {j}]");
                    return false;
                }
            }
        }
        return true;
    }

    public static int[] GenerateRandomArr(int size = 10000000, int min = 0, int max = int.MaxValue)
    {
        var arr = new int[size];
        var random = new Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = random.Next(min, max);
        }
        return arr;
    }
    
    public static string PathToHtmlFiles { get; } = "HTMLFiles";

    private static readonly string[] Dictionary = { 
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", 
        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", 
        "magna", "aliqua", "ut", "enim", "ad", "minim", "veniam", "quis", "nostrud" 
    };

    public static readonly string[] BlockTags = { "p", "div", "h1", "h2", "h3", "article", "section" };
    public static readonly string[] InlineTags = { "span", "b", "i", "strong", "em", "code", "mark", "small" };
    
    public static void GenerateHTMLFiles(int fileNumber = 1000, int minWordsNumber = 100, int maxWordsNumber = 500)
    {
        if (Directory.Exists(PathToHtmlFiles))
        {
            Directory.Delete(PathToHtmlFiles, true);
        }

        Directory.CreateDirectory(PathToHtmlFiles);
        var random = new Random();

        for (int i = 0; i < fileNumber; i++)
        {
            var fileName = Path.Combine(PathToHtmlFiles, $"{i}.html");
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine($"<head><title>Random content {i}</title></head>");
            sb.AppendLine("<body>");
            
            int wordsLimit = random.Next(minWordsNumber, maxWordsNumber + 1);
            int currentWords = 0;

            while (currentWords < wordsLimit)
            {
                string blockTag = BlockTags[random.Next(BlockTags.Length)];
                sb.Append($"<{blockTag}>");

                int wordsInBlock = random.Next(3, 15);
                for (int j = 0; j < wordsInBlock; j++)
                {
                    if (random.Next(0, 5) == 0) // 20% chance for inline tag
                    {
                        string inlineTag = InlineTags[random.Next(InlineTags.Length)];
                        sb.Append($"<{inlineTag}>{Dictionary[random.Next(Dictionary.Length)]}</{inlineTag}>");
                    }
                    else
                    {
                        sb.Append(Dictionary[random.Next(Dictionary.Length)]);
                    }
                    
                    if (j < wordsInBlock - 1) sb.Append(" ");
                }
                
                sb.Append($"</{blockTag}>");
                sb.AppendLine();
                currentWords += wordsInBlock;
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}