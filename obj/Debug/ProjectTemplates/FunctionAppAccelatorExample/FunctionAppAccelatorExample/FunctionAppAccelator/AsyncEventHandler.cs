using $safeprojectname$.Helpers;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace $safeprojectname$
{
    public class AsyncEventHandler
    {
        private readonly ILogger _logger;

        public AsyncEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AsyncApiHandler>();
        }

        [Function("EventHandler")]
        public async Task GetEvent([EventHubTrigger("%EventHubName%", Connection = "EventHubConnection", ConsumerGroup = "$Default")]
        string[] events)
        {

            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }
            _logger.LogInformation("C# EventHub trigger function received a request.");

            foreach (string eventData in events)
            {
                try
                {
                    CosmosHelper cosmosHelper = new();
                    await cosmosHelper.SaveEvent(eventData);
                    _logger.LogInformation("C# EventHub trigger function processed a request.");

                }
                catch (Exception e)
                {
                    _logger.LogError("C# EventHub trigger function failed to processed request. " + e.Message);
                }
            }
        }
    }
}

