using System;
using System.Net;
using System.Threading;
using System.Web.Http.SelfHost;
using EventStore.ClientAPI;
using WebAPI;

namespace ConsoleHost
{
    internal class ScatterGatherWireUp
    {
        private readonly ManualResetEventSlim _stop;
        private readonly string _baseAddress;
        private readonly IPEndPoint _eventStoreEndPoint;

        public ScatterGatherWireUp(string baseAddress, IPEndPoint eventStoreEndPoint, ManualResetEventSlim stop)
        {
            _baseAddress = baseAddress;
            _stop = stop;
            _eventStoreEndPoint = eventStoreEndPoint;
        }

        public void Run()
        {
            //Run Web API
            new Thread(RunWebApi).Start();
        }

        private void RunWebApi()
        {
            var webConnectionSettings = ConnectionSettings.Create()
		.UseConsoleLogger()
                .KeepReconnecting()
                .KeepRetrying();
            var webEventStoreConnection = EventStoreConnection.Create(webConnectionSettings, _eventStoreEndPoint,
                "es-web-connection");
            webEventStoreConnection.ConnectAsync().Wait();

            var config = new HttpSelfHostConfiguration(_baseAddress);
            new ApiBootstrapper().Configure(config, webEventStoreConnection);

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                _stop.Wait();
            }
        }
    }
}