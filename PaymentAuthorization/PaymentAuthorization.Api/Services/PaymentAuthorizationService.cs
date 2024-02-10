using PaymentAuthorization.Api.AsyncDataServices;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Enums;

namespace PaymentAuthorization.Api.Services
{

    public class PaymentAuthorizationService : IPaymentAuthorizationService
    {
        private readonly IMessageBusClient _messageBusClient;

        public PaymentAuthorizationService(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }

        public async Task<PaymentAuthorizationResponse> AuthorizePaymentAsync(PaymentAuthorizationRequest paymentRequest)
        {
            //enviar autorización a message bus
             _messageBusClient.PublishNewAuthorization(paymentRequest);
            
            return new PaymentAuthorizationResponse { Id = paymentRequest.Id, Status = AuthorizationStatusEnum.Pending }; 
        }
    }
}
