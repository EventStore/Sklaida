using System;
using System.Net;
using System.Threading;

namespace ConsoleHost
{
    internal class Program
    {
        const string BaseAddress = "http://localhost:9000";
        static readonly IPEndPoint EventStoreEndPoint = new IPEndPoint(IPAddress.Loopback, 1113);

        static void Main()
        {
            var stop = new ManualResetEventSlim(false);
       
            new ScatterGatherWireUp(BaseAddress, EventStoreEndPoint, stop).Run();

            Console.WriteLine("Press <ENTER> to stop server");
            Console.ReadLine();
            stop.Set();
        }

    }
}
