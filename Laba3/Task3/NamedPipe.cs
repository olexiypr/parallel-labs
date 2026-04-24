using System.IO.Pipes;
using System.Text;

namespace Laba3.Task3;

public class NamedPipe
{
    public void StartGeneratingNumbers(int numbersToSend = 10000)
    {
        using var pipe = new NamedPipeClientStream(".", "task3Pipe", PipeDirection.InOut, PipeOptions.None);
        pipe.Connect();
        var random = new Random();
        using var writer = new StreamWriter(pipe);
        writer.AutoFlush = true;
        using var reader = new StreamReader(pipe);
        var started = DateTime.Now.Ticks;
        Console.WriteLine("Started: " + started);
        for (int i = 0; i < numbersToSend; i++)
        {
            writer.Write(random.Next(2, 1000));
            var response = reader.ReadLine();
        }
        writer.Close();
        reader.Close();
        pipe.Close();
        /*writer.Write(random.Next(2, 1000));
        reader.ReadLine();*/
        var finished = DateTime.Now.Ticks;
        Console.WriteLine("Finished: " + finished);
        Console.WriteLine("Time: " + (finished - started) / 10000);
    }
}