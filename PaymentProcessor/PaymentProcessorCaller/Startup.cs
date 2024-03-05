using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentProcessor.Services;
using System;
using Serilog;
using System.Threading.Tasks;
using PaymentProcessorCaller.AsyncDataServices.Consumer;
using PaymentProcessorCaller.AsyncDataServices.Publisher;

namespace PaymentProcessor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Debug);  
            });

            services.AddSingleton<IPaymentAuthorizationProcessor, PaymentAuthorizationProcessor>();
            services.AddSingleton<IMessageBusPublisherClient, MessageBusPublisherClient>();

            services.AddMassTransit(x =>
            {
                    var rabbitMqOptions = Configuration.GetSection("RabbitMQ");
                    var host = rabbitMqOptions["Host"];
                    var port = Convert.ToInt32(rabbitMqOptions["Port"]);
                    var apiQueue = rabbitMqOptions["ApiQueue"];
                    var username = rabbitMqOptions["Username"];
                    var password = rabbitMqOptions["Password"];

                x.AddConsumer<PaymentProcessorConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint(apiQueue, c =>
                    {
                        c.ConfigureConsumer<PaymentProcessorConsumer>(context);
                    });

                });

            });

        }
    }

}
