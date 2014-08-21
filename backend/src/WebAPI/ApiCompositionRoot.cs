using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using EventStore.ClientAPI;
using WebAPI.Search;

namespace WebAPI
{
    public class ApiCompositionRoot : IHttpControllerActivator
    {
        private readonly IEventStoreConnection _connection;

        public ApiCompositionRoot(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == typeof (SearchController))
                return new SearchController(_connection);

            return null;
        }
    }
}