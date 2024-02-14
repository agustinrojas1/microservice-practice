using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.AsyncDataServices.Publisher
{
    public interface IMessageBusPublisherClient
    {
        Task PublishNewAuthorization(PaymentAuthorizationRequest paymentRequest);
    }
}
