namespace Laba3.Task3;

public class SharedFile
{
    public void StartGeneratingNumbers(int numbersToSend = 10000)
    {
        var path = @"C:\Users\Work\RiderProjects\ParallelLabs\Laba3\shared.data";
        var bufferSize = numbersToSend * sizeof(int);
        var mutex = new Mutex(true, "shared");
        Thread.Sleep(TimeSpan.FromSeconds(5));
        var started = DateTime.Now.Ticks;
        Console.WriteLine("Started: " + started);
        var file = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileOptions.WriteThrough);
        mutex.ReleaseMutex();
        for (int i = 1; i <= numbersToSend; i++)
        {
            var number = new Random().Next(2, 1000);
            var b = BitConverter.GetBytes(number);
            file.Write(b, 0, sizeof(int));
            if (i % 100 == 0)
            {
                Console.WriteLine(number);
                Console.WriteLine("Written: " + i);
            }
            file.Flush();
            var response = new byte[sizeof(int)];
            file.Seek( - sizeof(int), SeekOrigin.Current);
            while (BitConverter.ToInt32(response, 0) != number + 1)
            {
                file.Read(response, 0, sizeof(int));
                file.Seek( -sizeof(int), SeekOrigin.Current);
            }
            file.Seek(0, SeekOrigin.Begin);
        }
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);
    }
}