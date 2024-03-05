using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Enums;
using PaymentAuthorization.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcessorCaller.AsyncDataServices.Consumer
{
    public class PaymentResponseConsumer : IConsumer<PaymentAuthorizationResponse>
    {
        private readonly IPaymentAuthorizationService _authorizationService;
        private readonly ILogger<PaymentResponseConsumer> _logger;

        public PaymentResponseConsumer(IPaymentAuthorizationService authorizationService, ILogger<PaymentResponseConsumer> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentAuthorizationResponse> context)
        {
            var paymentResponse = context.Message;
            _logger.LogInformation("Payment response received: " + JsonConvert.SerializeObject(paymentResponse));
            await _authorizationService.UpdateAuthorizationStatusAsync(paymentResponse);

            if(paymentResponse.Status == AuthorizationStatusEnum.Authorized)
            {
                await _authorizationService.SaveAcceptedAuthorization(paymentResponse);
            }
            
        }
    }
}
