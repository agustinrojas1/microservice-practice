using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewAuthorization(PaymentAuthorizationRequest paymentRequest);
    }
}
