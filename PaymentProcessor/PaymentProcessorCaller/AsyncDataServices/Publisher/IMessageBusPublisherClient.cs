using PaymentAuthorization.Api.Models;

namespace PaymentProcessorCaller.AsyncDataServices.Publisher
{
    public interface IMessageBusPublisherClient
    {
        Task PublishNewAuthorizationResponse(PaymentAuthorizationResponse paymentResponse);
    }
}
