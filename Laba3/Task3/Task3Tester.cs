namespace Laba3.Task3;

public class Task3Tester
{
    public void TestPassingData()
    {
        var sharedMemory = new SharedMemory();
        var shredFile = new SharedFile();
        var namedPipe = new NamedPipe();
        var socket = new WebSockets();
        //sharedMemory.StartGeneratingNumbers();
        shredFile.StartGeneratingNumbers();
        //namedPipe.StartGeneratingNumbers();
        //socket.StartGeneratingNumbers().GetAwaiter().GetResult();
    }
}