using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WeatherDataIngestor
{
    public static class Function1
    {
        [FunctionName("ProcessWeatherData")]
        public static void Run([QueueTrigger("ibqueue", Connection = "ibsa01connstr")]string myQueueItem, ILogger log)
        {
            if (myQueueItem.Contains("exception")) throw new Exception("Exception found in message");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
