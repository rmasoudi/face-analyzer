using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace OpenFace.controller
{
    public class CORSFilter : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options) {
                
                return base.SendAsync(request, cancellationToken)
               .ContinueWith((task) =>
               {
                   HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK);
                   response.Headers.Add("Access-Control-Allow-Origin", "*");
                   response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                   response.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Origin, Content-Type, Origin, X-Auth-Token, clientId");
                   response.Headers.Add("Access-Control-Request-Headers", "Access-Control-Allow-Origin, Content-Type, Origin, X-Auth-Token, clientId");
                   return response;
               });
            }
            return base.SendAsync(request, cancellationToken)
                .ContinueWith((task) =>
                {
                    HttpResponseMessage response = task.Result;
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    response.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Origin, Content-Type, Origin, X-Auth-Token, clientId");
                    response.Headers.Add("Access-Control-Request-Headers", "Access-Control-Allow-Origin, Content-Type, Origin, X-Auth-Token, clientId");
                    return response;
                });
        }
    }
}
