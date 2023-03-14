using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAzureClients(builder =>
{
    builder.AddClient<QueueClient, QueueClientOptions>((options, _, _) =>
    {
        var conn = "DefaultEndpointsProtocol=https;AccountName=ibstrg01;AccountKey=xDlnVUK0yKgnSItLbvTWewWh6OjfNuZjRlbFlZFgRLgWePYaKGSRivIF9eYiLcMzw11lVmCudpr4+AStDhqOVA==;EndpointSuffix=core.windows.net";
        var queueName = "ibqueue";
        options.MessageEncoding = QueueMessageEncoding.Base64; //functions expect this to be base 64 encoded
        /*var credential = new DefaultAzureCredential();
        var queueUri = new Uri("https://ibsa01.queue.core.windows.net/ibqueue");
        return new QueueClient(queueUri, credential, options);*/
        return new QueueClient(conn, queueName, options);
    });
});


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
