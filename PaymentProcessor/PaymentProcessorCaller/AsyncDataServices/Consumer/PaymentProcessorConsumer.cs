using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentAuthorization.Api.Models;
using PaymentProcessor.Services;
using PaymentProcessorCaller.AsyncDataServices.Publisher;

namespace PaymentProcessorCaller.AsyncDataServices.Consumer
{
    public class PaymentProcessorConsumer : IConsumer<PaymentAuthorizationRequest>
    {
        private readonly IPaymentAuthorizationProcessor _paymentProcessor;
        private readonly IMessageBusPublisherClient _messageBusPublisherClient;
        private readonly ILogger<PaymentProcessorConsumer> _logger;

        public PaymentProcessorConsumer(IPaymentAuthorizationProcessor paymentProcessor, IMessageBusPublisherClient messageBusPublisherClient, ILogger<PaymentProcessorConsumer> logger)
        {
            _paymentProcessor = paymentProcessor;
            _messageBusPublisherClient = messageBusPublisherClient;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentAuthorizationRequest> context)
        {
            var message = context.Message;
            _logger.LogInformation("Payment Request received for authorization: " + JsonConvert.SerializeObject(message));

            var response = _paymentProcessor.ProcessAuthorization(message);

            await _messageBusPublisherClient.PublishNewAuthorizationResponse(response);

        }
    }
}
