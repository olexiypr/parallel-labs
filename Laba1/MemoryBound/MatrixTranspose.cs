namespace Laba1.MemoryBound;

public class MatrixTranspose(int threadsCount) : BaseTask(threadsCount)
{
    public void Transpose(int[,] matrix, int sizeX, int sizeY = 0)
    {
        var res = new int[sizeY, sizeX];
        Console.WriteLine("Start working with " + ThreadsCount + " threads");
        using CountdownEvent countdown = new(ThreadsCount);
        for (int f = 0; f < ThreadsCount; f++)
        {
            var index = f;
            var thread = new Thread(() =>
            {
                for (int i = index; i < sizeX; i+= ThreadsCount)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        res[j, i] = matrix[i, j];
                    }
                }
                countdown.Signal();
            });
            thread.Start();
        }
        
        countdown.Wait();
        
        //PrintMatrix(res);
    }
    
    public int[,] GetMatrix(int sizeX, int sizeY = 0)
    {
        ThrowIfNotMultipleInput(sizeX);
        ThrowIfNotMultipleInput(sizeY);
        sizeY = sizeY == 0 ? sizeX : sizeY;
        var matrix = new int[sizeX, sizeY];
        FillMatrix(matrix);
        return matrix;
    }

    private void PrintMatrix(int[,] matrix)
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

    private void FillMatrix(int[,] matrix)
    {
        var random = new Random();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = random.Next(0, 10);
            }
        }
    }
}