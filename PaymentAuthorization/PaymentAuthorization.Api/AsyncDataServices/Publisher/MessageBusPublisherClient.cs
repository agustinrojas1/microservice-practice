using MassTransit;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using PaymentAuthorization.Api.Models;
using RabbitMQ.Client;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace PaymentAuthorization.Api.AsyncDataServices.Publisher
{
    public class MessageBusPublisherClient : IMessageBusPublisherClient
    {
        private readonly IConfiguration _configuration;
        private readonly IBusControl _busControl;

        public MessageBusPublisherClient(IConfiguration configuration, IBusControl busControl)
        {
            _configuration = configuration;
            _busControl = busControl;
            _busControl.StartAsync();

        }
        public async Task PublishNewAuthorization(PaymentAuthorizationRequest paymentRequest)
        {
                var rabbitMqOptions = _configuration.GetSection("RabbitMQ");
                
                var apiQueue = rabbitMqOptions["ApiQueue"];
                var host = rabbitMqOptions["Host"];
                var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"rabbitmq://{host}/{apiQueue}"));
                await sendEndpoint.Send(paymentRequest);

                Console.WriteLine("RabbitMQ Connection Open");
        }

        public void Dispose()
        {
            _busControl.StopAsync();
            Console.WriteLine("Message Bus disposed");
        }
        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Shutdown");
        }

    }
}
