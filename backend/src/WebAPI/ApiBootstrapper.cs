using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WebAPI
{
    public class ApiBootstrapper
    {
        public void Configure(HttpConfiguration config, IEventStoreConnection eventStoreConnection)
        {
	    config.Services.Replace(typeof(IHttpControllerActivator), new ApiCompositionRoot(eventStoreConnection));

	    config.Formatters.Clear();
	    config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
	    config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            config.Routes.MapHttpRoute("InputQueue", "search/", new { Controller = "Search" });
        }
    }
}