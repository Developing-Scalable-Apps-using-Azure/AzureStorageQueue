using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureQueueDemo01
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddAzureClients(builder =>
            {
                builder.AddClient<QueueClient, QueueClientOptions>((options, _, _) =>
                {
                    var conn = "DefaultEndpointsProtocol=https;AccountName=ibsa01;AccountKey=87hgTg8Fkev8pHMGSP6OL7BO7sbjdGPo2bZOt2dK9ZLtFGYGMpIb+Hk/SokYZ2a8Olo9Bnbvw2uGkM5LjcOYAw==;EndpointSuffix=core.windows.net";
                    var queueName = "ibqueue";
                    options.MessageEncoding = QueueMessageEncoding.Base64; //functions expect this to be base 64 encoded
                    /*var credential = new DefaultAzureCredential();
                    var queueUri = new Uri("https://ibsa01.queue.core.windows.net/ibqueue");
                    return new QueueClient(queueUri, credential, options);*/
                    return new QueueClient(conn, queueName, options);
                });
            });
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "WeatherForecasts",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
