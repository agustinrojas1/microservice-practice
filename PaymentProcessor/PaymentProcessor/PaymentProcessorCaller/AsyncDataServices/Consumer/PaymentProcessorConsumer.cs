using MassTransit;
using PaymentAuthorization.Api.Models;
using PaymentProcessor.Services;
using PaymentProcessorCaller.AsyncDataServices.Publisher;

namespace PaymentProcessorCaller.AsyncDataServices.Consumer
{
    public class PaymentProcessorConsumer : IConsumer<PaymentAuthorizationRequest>
    {
        private readonly IPaymentAuthorizationProcessor _paymentProcessor;
        private readonly IMessageBusPublisherClient _messageBusPublisherClient;

        public PaymentProcessorConsumer(IPaymentAuthorizationProcessor paymentProcessor, IMessageBusPublisherClient messageBusPublisherClient)
        {
            _paymentProcessor = paymentProcessor;
            _messageBusPublisherClient = messageBusPublisherClient;
        }

        public async Task Consume(ConsumeContext<PaymentAuthorizationRequest> context)
        {
            var message = context.Message;

            var response = _paymentProcessor.ProcessAuthorization(message);

            await _messageBusPublisherClient.PublishNewAuthorizationResponse(response);

            //await context.RespondAsync(response);
        }
    }
}
