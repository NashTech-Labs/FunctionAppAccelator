using System.Net;
using Azure;
using $safeprojectname$.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$
{
    public class AsyncApiHandler
    {
        private readonly ILogger _logger;

        public AsyncApiHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AsyncApiHandler>();
        }

        [Function("AsyncTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/async")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                if (req == null)
                {
                    throw new ArgumentNullException(nameof(req));
                }
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                EventHelper eventHelper = new();
                await eventHelper.SendEvent(requestBody);
                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("event sent successfully");
            }          
            catch (Exception ex) 
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                _logger.LogError("Exception--- "+ex.Message);
            }
            return response;
        }
    }
}
