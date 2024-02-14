using PaymentAuthorization.Api.Models;


namespace PaymentProcessor.Services
{
    public class PaymentAuthorizationProcessor : IPaymentAuthorizationProcessor
    {
        public PaymentAuthorizationResponse ProcessAuthorization(PaymentAuthorizationRequest paymentAuthorization)
        {
            int amount;
            bool isInt = int.TryParse(paymentAuthorization.Amount, out amount);
            var isAutorized = isInt ? AuthorizationStatusEnum.Authorized : AuthorizationStatusEnum.Rejected;
            var message = isInt ? "Payment authorized" : "Payment rejected";

            return new PaymentAuthorizationResponse { Id = paymentAuthorization.Id,  Status = isAutorized, Message = message };

        }
    }


}
