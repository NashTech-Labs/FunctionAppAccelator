using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$.Helpers
{
    public class CosmosHelper
    {
        private readonly ILogger _logger;

        public CosmosHelper() { }
        public CosmosHelper(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CosmosHelper>();
        }
        private string EndpointUrl = "https://demotest01.documents.azure.com:443/";
        private string PrimaryKey = Environment.GetEnvironmentVariable("CosmosKey");
        private const string DatabaseId = "test001";
        private const string ContainerId = "eventstore";
        public async Task SaveEvent(string events)
        {

            CosmosClient cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/eventId");
            EventData eventData = new EventData
            {
                id = Guid.NewGuid().ToString(),
                eventId = Guid.NewGuid().ToString(),
                Data = events
            };

            await SaveEventToCosmosDb(container, eventData);
        }
         private async Task SaveEventToCosmosDb(Container container, EventData eventData)
        {
            try
            {
                ItemResponse<EventData> response = await container.CreateItemAsync(eventData, new PartitionKey(eventData.eventId));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error saving event: {ex.Message}");
            }
        }
        public class EventData
        {
            public string id { get; set; }
            public string eventId { get; set; }
            public string Data { get; set; }
        }
    }
}
