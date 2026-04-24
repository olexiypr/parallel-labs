using Laba1;

namespace Laba2.Task1;

public class Task1Tester : BaseTest
{
    private readonly int[] TasksNumbers = [2, 10, 20, 40, 100];
    private readonly string singleThreadName = " | SingleThread";
    private readonly string mapReduceName = " | MapReduce";
    private readonly string forkJoinName = " | ForkJoin";
    private readonly string workerPoolName = " | WorkerPool";
    
    public void TestHtmlTagsFrequency()
    {
        var singleThreadAlg = new SequentialAlg();
        var mapReduce = new MapReduce();
        var forkJoin = new ForkJoin();
        var workerPool = new WorkerPool();

        var name = "HtmlTagsFrequency";
        
        LogBeginTest(name, "");
        TestAction(() => singleThreadAlg.CalculateHtmlTagsFrequency(), name + singleThreadName, 1);
        
        foreach (var tasksNumber in TasksNumbers)
        {
            TestAction(() => mapReduce.CalculateHtmlTagsFrequency(tasksNumber), name + mapReduceName, tasksNumber);
            TestAction(() => forkJoin.CalculateHtmlTagsFrequency(tasksNumber), name + forkJoinName, tasksNumber);
            TestAction(() => workerPool.CalculateHtmlTagsFrequency(tasksNumber), name + workerPoolName, tasksNumber);
        }

        LogCompletedTest(name);
    }

    public void TestArrayProcessing()
    {
        var singleThreadAlg = new SequentialAlg();
        var mapReduce = new MapReduce();
        var forkJoin = new ForkJoin();
        var workerPool = new WorkerPool();
        var name = "ArrayProcessing";
        var arrSizes = new int[] { 40000000 };
        foreach (var arrSize in arrSizes)
        {
            var arr = Helper.GenerateRandomArr(arrSize);
            LogBeginTest(name, arrSize); 
            TestAction(() => singleThreadAlg.ProcessArray(arr,true), name + singleThreadAlg, 1);

            foreach (var tasksNumber in TasksNumbers)
            {
                TestAction(() => mapReduce.ProcessArray(arr, tasksNumber), name + mapReduce, tasksNumber);
                TestAction(() => forkJoin.ProcessArray(arr, tasksNumber), name + forkJoin, tasksNumber);
                TestAction(() => workerPool.ProcessArray(arr, tasksNumber), name + workerPool, tasksNumber);
            }

            LogCompletedTest(name);
        }
    }

    public void TestMatrixProcessing()
    {
        var singleThreadAlg = new SequentialAlg();
        var mapReduce = new MapReduce();
        var forkJoin = new ForkJoin();
        var workerPool = new WorkerPool();
        var name = "MatrixProcessing";
        int[,] matrix1 = {
            { 7, 4, 8, 5, 7, 3 },
            { 7, 8, 5, 4, 8, 8 },
            { 3, 6, 5, 2, 8, 6 },
            { 2, 5, 1, 6, 9, 1 },
            { 3, 7, 4, 9, 3, 5 },
            { 3, 7, 5, 9, 7, 2 }
        };
        int[,] matrix2 = {
            { 4, 9, 2, 9, 5, 2 },
            { 4, 7, 8, 3, 1, 4 },
            { 2, 8, 4, 2, 6, 6 },
            { 4, 6, 2, 2, 4, 8 },
            { 7, 9, 8, 5, 2, 5 },
            { 8, 9, 9, 1, 9, 7 }
        };
        var matrixSizes = new int[] {4000};
        foreach (var matrixSize in matrixSizes)
        {
            var sizeX = matrixSize;
            var sizeY = matrixSize;
            matrix1 = Helper.GenerateRandomMatrix(sizeX, sizeY);
            matrix2 = Helper.GenerateRandomMatrix(sizeX, sizeY);
            LogBeginTest(name, matrixSize);
            var matrix3 = matrix1;
            var matrix4 = matrix2;
            
            var matrix5 = matrix1;
            var matrix6 = matrix2;
            
            var matrix7 = matrix1;
            var matrix8 = matrix2;
            
            var matrix9 = matrix1;
            var matrix10 = matrix2;
            
            //TestAction(() => singleThreadAlg.MultiplyMatrix(matrix3, matrix4), name + singleThreadAlg, 1);
            
            foreach (var tasksNumber in TasksNumbers)
            {
                //TestAction(() => mapReduce.MultiplyMatrix(matrix5, matrix6, tasksNumber), name + mapReduce, tasksNumber);
                TestAction(() => forkJoin.MultiplyMatrix(matrix7, matrix8, tasksNumber), name + forkJoin, tasksNumber);
                //TestAction(() => workerPool.MultiplyMatrix(matrix9, matrix10), name + workerPool, tasksNumber);
            }
            
            LogCompletedTest(name);
        }
    }
    //TestAction(() => workerPool.CalculateHtmlTagsFrequencyCustomPool());
}