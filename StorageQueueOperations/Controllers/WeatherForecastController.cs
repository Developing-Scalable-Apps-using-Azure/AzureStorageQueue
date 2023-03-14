using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StorageQueueOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureQueueDemo01.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly QueueClient _queueClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, QueueClient queueClient)
        {
            _logger = logger;
            _queueClient = queueClient;
        }

        [HttpGet]
        [Route("/getForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Route("/")]
        public async Task post([FromBody] WeatherForecast data)
        {
            /*var conn = "DefaultEndpointsProtocol=https;AccountName=ibstrg01;AccountKey=xDlnVUK0yKgnSItLbvTWewWh6OjfNuZjRlbFlZFgRLgWePYaKGSRivIF9eYiLcMzw11lVmCudpr4+AStDhqOVA==;EndpointSuffix=core.windows.net";
            var queueName = "ibqueue";
            var queueClient = new QueueClient(conn, queueName);*/
            var message = JsonSerializer.Serialize(data);
            //await queueClient.SendMessageAsync(message, null, TimeSpan.FromSeconds(10)); //time to live = expiry time
            //await queueClient.SendMessageAsync(message, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)); //visibility timeout
            //await queueClient.SendMessageAsync(message, null, TimeSpan.FromSeconds(-1)); // -ve value = no expiry
            await _queueClient.SendMessageAsync(message, null, TimeSpan.FromSeconds(-1));

        }
    }
}
