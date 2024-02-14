using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Enums;

namespace PaymentAuthorization.Api.Services
{

    public class PaymentAuthorizationService : IPaymentAuthorizationService
    {
        private readonly IMessageBusPublisherClient _messageBusClient;

        public PaymentAuthorizationService(IMessageBusPublisherClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }

        public async Task<PaymentAuthorizationResponse> AuthorizePaymentAsync(PaymentAuthorizationRequest paymentRequest)
        {
            //enviar autorización a message bus
             await _messageBusClient.PublishNewAuthorization(paymentRequest);
            
            return new PaymentAuthorizationResponse { Id = paymentRequest.Id, Status = AuthorizationStatusEnum.Pending, Message = "Your payment authorization has been sent. Please wait for the confirmation." }; 
        }
    }
}
