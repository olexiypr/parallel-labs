using System.IO.MemoryMappedFiles;
using System.Text;

namespace Laba3.Task3;

public class SharedMemory
{
    public void StartGeneratingNumbers(int numbersToSend = 10000)
    {
        var bufferSize = numbersToSend * sizeof(int);
        var mutex = new Mutex(true, "shared");
        Thread.Sleep(TimeSpan.FromSeconds(5));
        
        var started = DateTime.Now.Ticks;
        long currentPosition = 1;
        long flagPosition = 0;
        byte flag = 1;
        using var mmf = MemoryMappedFile.CreateNew("SharedData", bufferSize);
        using var accessor = mmf.CreateViewAccessor(currentPosition, bufferSize);
        Console.WriteLine("Started: " + started);
        mutex.ReleaseMutex();
        for (int i = 1; i <= numbersToSend; i++)
        {
            var number = new Random().Next(2, 1000);
            accessor.Write(currentPosition, number);
            accessor.Write(flagPosition, flag);
            if (i % 100 == 0)
            {
                Console.WriteLine(number);
                Console.WriteLine("Written: " + i);
            }
            while (accessor.ReadByte(flagPosition) != 0)
            {
                
            }
            var result = accessor.ReadInt32(currentPosition);
            if (result != number + 1)
            {
                Console.WriteLine("Error: " + result + " != " + (number + 1));
            }
        }
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);
    }
}