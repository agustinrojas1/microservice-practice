using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentAuthorization.Api.Models;
using RabbitMQ.Client;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace PaymentProcessorCaller.AsyncDataServices.Publisher
{
    public class MessageBusPublisherClient : IMessageBusPublisherClient
    {
        private readonly IConfiguration _configuration;
        private readonly IBusControl _busControl;
        private readonly ILogger<MessageBusPublisherClient> _logger;

        public MessageBusPublisherClient(IConfiguration configuration, IBusControl busControl, ILogger<MessageBusPublisherClient> logger)
        {
            _configuration = configuration;
            _busControl = busControl;
            _busControl.StartAsync();
            _logger = logger;

        }
        public async Task PublishNewAuthorizationResponse(PaymentAuthorizationResponse paymentResponse)
        {
                var rabbitMqOptions = _configuration.GetSection("RabbitMQ");
                
                var apiQueue = rabbitMqOptions["ResponseQueue"];
                var host = rabbitMqOptions["Host"];
                var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"rabbitmq://{host}/{apiQueue}"));
                await sendEndpoint.Send(paymentResponse);

                _logger.LogInformation("Authorization response sent to message bus: " + JsonConvert.SerializeObject(paymentResponse));
        }

        public void Dispose()
        {
            _busControl.StopAsync();
            _logger.LogInformation("Message Bus disposed");

        }
        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMQ Connection Shutdown");

        }

    }
}
