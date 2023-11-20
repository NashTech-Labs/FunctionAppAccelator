using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace $safeprojectname$.Helpers
{
    public class EventHelper
    {
        public virtual async Task SendEvent(string eventData)
        {
            var producerClient = GetEventHubProducer();
            EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventData)));
            await producerClient.SendAsync(eventBatch);
        }
        private EventHubProducerClient GetEventHubProducer()
        {
            return new EventHubProducerClient(Environment.GetEnvironmentVariable("EventHubConnection"), Environment.GetEnvironmentVariable("EventHubName"));
        }
    }
}
