using System;
using System.Threading;

namespace ConsoleHost
{
    internal class Program
    {
        static void Main()
        {
            const string baseAddress = "http://localhost:9000";
            var stop = new ManualResetEventSlim(false);

            new ScatterGatherWireUp(baseAddress, stop).Run();

            Console.WriteLine("Press <ENTER> to stop server");
            Console.ReadLine();
            stop.Set();
        }

    }
}
