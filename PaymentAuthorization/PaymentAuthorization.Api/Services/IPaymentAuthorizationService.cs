using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.Services
{
    public interface IPaymentAuthorizationService
    {
        Task<PaymentAuthorizationResponse> AuthorizePaymentAsync(PaymentAuthorizationRequest paymentRequest);
    }
}
