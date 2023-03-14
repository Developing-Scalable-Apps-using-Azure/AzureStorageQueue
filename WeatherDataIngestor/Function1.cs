using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WeatherDataIngestor
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("ibqueue", Connection = "ibstrg01connstr")]string myQueueItem, ILogger log)
        {
            if (myQueueItem.Contains("exception")) throw new Exception("Exception found in message");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
