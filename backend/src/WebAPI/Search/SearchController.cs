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

            //This will set the stream ACL to allow reads from all but only writes from
            //the admins group. With more secure data you may limit reads only to the user
            //that made the request and likely a backenduser who is likely not a member 
            //of the $admins group for security purposes.
            //
            //await _eventStoreConnection.SetStreamMetadataAsync(resultStream,
            //                                       ExpectedVersion.EmptyStream,
            //                                       StreamMetadata.Build()
            //                                                    .SetMaxAge(TimeSpan.FromDays(90))
            //                                                    .SetWriteRole("backenduser")
            //                                                    .SetReadRole(Request.User),
            //                                       new UserCredentials("backenduser", "password"));
            //
            //This code also sets an expiration on the stream of 5 minutes. More than
            //likely in a production system you would not want such a short expiration
            //this is more so to be able to show the functionality of expiring the
            //results over time in a way that can actually be demoed. In most such
            //systems this would likely be months or possibly even never due to 
            //operational needs of being able to see what happened with a given 
            //request.
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