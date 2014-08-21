using System.Threading;
using System.Web.Http.SelfHost;
using WebAPI;

namespace ConsoleHost
{
    internal class ScatterGatherWireUp
    {
        private readonly ManualResetEventSlim _stop;
        private readonly string _baseAddress;

        public ScatterGatherWireUp(string baseAddress, ManualResetEventSlim stop)
        {
            _baseAddress = baseAddress;
            _stop = stop;
        }

        public void Run()
        {
            //Run Web API
            new Thread(RunWebApi).Start();
        }

        private void RunWebApi()
        {
            var config = new HttpSelfHostConfiguration(_baseAddress);
            new ApiBootstrapper().Configure(config);

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                _stop.Wait();
            }
        }
    }
}