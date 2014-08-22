using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Http.SelfHost;
using BackendServices;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using WebAPI;

namespace ConsoleHost
{
    internal class ScatterGatherWireUp
    {
        private readonly ManualResetEventSlim _stop;
        private readonly string _baseAddress;
        private readonly IPEndPoint _eventStoreEndPoint;
        private readonly Dictionary<string, OuroSellerEndpointAdapter> _backendAdapters = new Dictionary<string, OuroSellerEndpointAdapter>();
        private UserCredentials _credentials = new UserCredentials("admin", "changeit");
        private readonly string _incomingStream = "incoming";

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
            RunBackend();
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

        private void RunBackend()
        {
            var backendSettings = ConnectionSettings.Create()
                .UseConsoleLogger()
                .KeepReconnecting()
                .KeepRetrying();
            var connection = EventStoreConnection.Create(backendSettings, _eventStoreEndPoint,
                "es-backend-connection");
            connection.ConnectAsync().Wait();
            
            _backendAdapters.Add("declining1", 
                                 new OuroSellerEndpointAdapter(connection, 
                                                              "declining1", 
                                                              new DecliningOuroSellerEndpoint("crap company", new Uri("http://google.com")),
                                 _incomingStream, _credentials));    

	    foreach (var adapter in _backendAdapters.Values)
		adapter.StartListening();
        }

        public void StopBackend()
        {
            foreach (var a in _backendAdapters.Values)
            {
                a.Stop();
            }
        }
    }
}