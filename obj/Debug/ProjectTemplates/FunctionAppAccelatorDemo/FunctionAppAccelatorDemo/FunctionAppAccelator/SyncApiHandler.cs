using $safeprojectname$.Helpers;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;


namespace $safeprojectname$
{
    public class SyncApiHandler
    {
        private readonly ILogger _logger;

        public SyncApiHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AsyncApiHandler>();
        }

        [Function("SyncTrigger")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/sync")] HttpRequestData req)
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
                CosmosHelper cosmosHelper = new();
                cosmosHelper.SaveEvent(requestBody).Wait();
                _logger.LogInformation("C# HTTP trigger function processed successfully.");
                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("event sent successfully");

            }
            catch (Exception ex)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                _logger.LogError("Exception--- " + ex.Message);
            }
            return response;
        }
    }
}
