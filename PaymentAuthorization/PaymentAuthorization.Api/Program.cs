using MassTransit;
using Microsoft.Extensions.Configuration;
using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Services;
using PaymentProcessorCaller.AsyncDataServices.Consumer;
using Serilog;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPaymentAuthorizationService, PaymentAuthorizationService>();
builder.Services.AddSingleton<IMessageBusPublisherClient, MessageBusPublisherClient>();
builder.Services.AddSingleton<IPaymentAuthorizationRepository, PaymentAuthorizationRepository>();

builder.Services.AddMassTransit(x =>
{
    var rabbitMqOptions = builder.Configuration.GetSection("RabbitMQ");
    var host = rabbitMqOptions["Host"];
    var port = int.Parse(rabbitMqOptions["Port"]);
    var username = rabbitMqOptions["Username"];
    var password = rabbitMqOptions["Password"];   
    var responseQueue = rabbitMqOptions["ResponseQueue"];
    x.AddConsumer<PaymentResponseConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
        {
            h.Username(username);
            h.Password(password);
        });
        cfg.ReceiveEndpoint(responseQueue, c =>
        {
            c.ConfigureConsumer<PaymentResponseConsumer>(context);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
