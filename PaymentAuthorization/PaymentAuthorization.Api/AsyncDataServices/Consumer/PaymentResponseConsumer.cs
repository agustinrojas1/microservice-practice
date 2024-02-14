using MassTransit;
using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcessorCaller.AsyncDataServices.Consumer
{
    public class PaymentResponseConsumer : IConsumer<PaymentAuthorizationResponse>
    {

        public async Task Consume(ConsumeContext<PaymentAuthorizationResponse> context)
        {
            var message = context.Message;

            //UPDATE PaymentAuthorization - repo

            
        }
    }
}
