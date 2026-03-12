namespace Laba1.MemoryBound;

public class MemoryBound : BaseTest
{
    public void TestTranspose()
    {
        var matrixTranspose = new MatrixTranspose(20);
        var matrixTransposeSingle = new MatrixTranspose(1);
        var sizeX = 100000;
        var sizeY = 30000;
        var matrix = matrixTranspose.GetMatrix(sizeX, sizeY);
        TestAction(() => matrixTransposeSingle.Transpose(matrix, sizeX, sizeY));
        TestAction(() => matrixTranspose.Transpose(matrix, sizeX, sizeY));
    }
}