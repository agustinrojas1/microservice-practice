using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Enums;

namespace PaymentAuthorization.Api.Services
{

    public class PaymentAuthorizationService : IPaymentAuthorizationService
    {
        private readonly IMessageBusPublisherClient _messageBusClient;
        private readonly IPaymentAuthorizationRepository _authorizationRepository;
        private readonly IAcceptedAuthorizationRepository _acceptedAuthorizationRepository;
        private readonly ILogger<PaymentAuthorizationService> _logger;

        public PaymentAuthorizationService(IMessageBusPublisherClient messageBusClient, IPaymentAuthorizationRepository authorizationRepository, IAcceptedAuthorizationRepository acceptedAuthorizationRepository, ILogger<PaymentAuthorizationService> logger)
        {
            _messageBusClient = messageBusClient;
            _authorizationRepository = authorizationRepository;
            _acceptedAuthorizationRepository = acceptedAuthorizationRepository;
            _logger = logger;
        }

        public async Task<PaymentAuthorizationResponse> AuthorizePaymentAsync(PaymentAuthorizationRequest paymentRequest)
        {
            _logger.LogInformation("Authorizing payment request: " + JsonConvert.SerializeObject(paymentRequest));
             await _messageBusClient.PublishNewAuthorization(paymentRequest);

            if (paymentRequest.ClientType == ClientTypeEnum.Second)
            {
                await StartReversalProcess(paymentRequest);
            }

            return new PaymentAuthorizationResponse { AuthorizationRequestId = paymentRequest.Id, Status = AuthorizationStatusEnum.Pending, Message = "Your payment authorization has been sent. Please wait for the confirmation." }; 
        }

        private async Task StartReversalProcess(PaymentAuthorizationRequest paymentRequest)
        {
            _logger.LogInformation("Starting reversal process.");

            //cambiar a FromMinutes(5)
            await Task.Delay(TimeSpan.FromMilliseconds(1));

            _logger.LogInformation("The time limit has passed.");
            var authorization = await _authorizationRepository.GetAuthorizationById(paymentRequest.Id);

            _logger.LogInformation("authorization to be reversed (or not): " + JsonConvert.SerializeObject(authorization));

            if (authorization != null && authorization.Status != AuthorizationStatusEnum.Authorized && authorization.AuthorizationType != AuthorizationTypeEnum.Reversal)
            {
                await ReverseAuthorization(authorization);
            }
        }
        private async Task ReverseAuthorization(PaymentAuthorizationRequest authorization)
        {
            _logger.LogInformation("Reversing authorization.");

            var reversalAuthorization = new PaymentAuthorizationRequest
            {
                ClientId = authorization.ClientId,
                Amount = authorization.Amount,
                AuthorizationType = AuthorizationTypeEnum.Reversal,
                ClientType = authorization.ClientType
            };
            _logger.LogInformation("Saving reversal authorization.");

            await _authorizationRepository.SaveAuthorization(reversalAuthorization);

            _logger.LogInformation("Publishing reversal authorization.");

            await _messageBusClient.PublishNewAuthorization(reversalAuthorization);
        }

        public async Task UpdateAuthorizationStatusAsync(PaymentAuthorizationResponse paymentResponse)
        {
            _logger.LogInformation("updating authorization status from payment response: " + JsonConvert.SerializeObject(paymentResponse));
            var paymentAuthorization = await _authorizationRepository.GetAuthorizationById(paymentResponse.AuthorizationRequestId);

            if(paymentAuthorization != null)
            {
                paymentAuthorization.Status = paymentResponse.Status;
                await _authorizationRepository.UpdateAuthorization(paymentAuthorization);
            }

        }

        public async Task SaveAcceptedAuthorization(PaymentAuthorizationResponse paymentResponse)
        {
            try
            {
                _logger.LogInformation("Saving authorization to Accepted Authorizations: " + JsonConvert.SerializeObject(paymentResponse));

                var paymentRequest = await _authorizationRepository.GetAuthorizationById(paymentResponse.AuthorizationRequestId);

                if (paymentRequest != null)
                {
                    _logger.LogInformation("Payment request to be saved as accepted authorization: " + JsonConvert.SerializeObject(paymentRequest));

                    var acceptedAuthorization = new AcceptedPaymentAuthorization
                    {
                        Amount = int.Parse(paymentRequest.Amount),
                        ClientId = paymentRequest.ClientId,
                    };

                    await _acceptedAuthorizationRepository.SaveAcceptedAuthorization(acceptedAuthorization);
                }
                else
                    _logger.LogError($"Payment request with id {paymentResponse.AuthorizationRequestId} can't be found. Nothing was saved.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex}");
            }
        }
    }
}
