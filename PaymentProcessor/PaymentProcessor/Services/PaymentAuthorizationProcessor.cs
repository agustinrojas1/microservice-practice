using Microsoft.Extensions.Logging;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Enums;


namespace PaymentProcessor.Services
{
    public class PaymentAuthorizationProcessor : IPaymentAuthorizationProcessor

    {
        private readonly ILogger<PaymentAuthorizationProcessor> _logger;

        public PaymentAuthorizationProcessor(ILogger<PaymentAuthorizationProcessor> logger)
        {
            _logger = logger;
        }
        public PaymentAuthorizationResponse ProcessAuthorization(PaymentAuthorizationRequest paymentAuthorization)
        {
            _logger.LogInformation("Processing payment request");
            int amount;
            bool isInt = int.TryParse(paymentAuthorization.Amount, out amount);
            var isAutorized = isInt ? AuthorizationStatusEnum.Authorized : AuthorizationStatusEnum.Rejected;
            var message = isInt ? "Payment authorized" : "Payment rejected";

            _logger.LogInformation("Processing result: " + message);

            return new PaymentAuthorizationResponse { AuthorizationRequestId = paymentAuthorization.Id,  Status = isAutorized, Message = message};

        }
    }


}
