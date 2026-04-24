namespace Laba2.Task1;

public class MatrixResult
{
    public int[,] Matrix { get; set; }
    public int BeginIndex { get; set; }
    public int EndIndex { get; set; }

    public MatrixResult(int[,] matrix, int beginIndex, int endIndex)
    {
        Matrix = matrix;
        BeginIndex = beginIndex;
        EndIndex = endIndex;
    }
}