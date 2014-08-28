using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using WebAPI.Shared;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;

namespace WebAPI.Search
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "Location")]
    public class SearchController : ApiController
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public SearchController(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            var formatter = controllerContext.Configuration.Formatters.JsonFormatter;
            formatter.SupportedMediaTypes.Clear();
	    formatter.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/vnd.ouroinc.searchrequest+json"));
        }

        public async Task<HttpResponseMessage> Post(OuroSearchRequest request)
        {
            var id = Guid.NewGuid();
            var resultStream = string.Format("searchresult-{0}", id.ToString("N"));
            var responseUri = string.Format("search-result/{0}", id.ToString("N"));

            await _eventStoreConnection.SetStreamMetadataAsync(resultStream, 
                                                               ExpectedVersion.EmptyStream,
                                                               StreamMetadata.Build()
                                                                            .SetMaxAge(TimeSpan.FromMinutes(5))
                                                                            .SetWriteRole("$admins")
                                                                            .SetReadRole("$all"), 
                                                               new UserCredentials("admin", "changeit"));
            await _eventStoreConnection.AppendToStreamAsync("incoming", ExpectedVersion.Any,
                request.ToEvent(resultStream).ToEventData("searchRequested"));

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            response.Headers.Location = new Uri(responseUri, UriKind.Relative);
            return response;
        }
    }
}