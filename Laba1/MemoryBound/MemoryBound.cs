namespace Laba1.MemoryBound;

public record MatrixSize(int X, int Y);
public class MemoryBound : BaseTest
{
    public void TestTranspose()
    {
        var name = "Transpose";
        var threadsCounts = new int[] {1, 2, 5, 10, 20, 40, 100};
        List<MatrixSize> matrixSizes = [new MatrixSize(10000, 30000), new MatrixSize(20000, 60000), new MatrixSize(40000, 30000 * 4)];
        foreach (var matrixSize in matrixSizes)
        {
            var matrixTransposeSingle = new MatrixTranspose(1);
            var matrix = matrixTransposeSingle.GetMatrix(matrixSize.X, matrixSize.Y);
            LogBeginTest(name, matrixSize.X + "x" + matrixSize.Y);
            foreach (var threadsCount in threadsCounts)
            {
                var matrixTranspose = new MatrixTranspose(threadsCount);
                TestAction(() => matrixTranspose.Transpose(matrix, matrixSize.X, matrixSize.Y), name, threadsCount);
            }
            LogCompletedTest(name);
        }
    }
}