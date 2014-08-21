using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebAPI.Search
{
    public class SearchController : ApiController
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            var formatter = controllerContext.Configuration.Formatters.JsonFormatter;
            formatter.SupportedMediaTypes.Clear();
	    formatter.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/vnd.ouroinc.searchrequest+json"));
        }

        public async Task<HttpResponseMessage> Post(OuroSearchRequest request)
        {
            await Task.Delay(100);

            var id = Guid.NewGuid();
            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            response.Headers.Location = new Uri(string.Format("search-result/{0}", id.ToString("N")), UriKind.Relative);
            return response;
        }
    }
}