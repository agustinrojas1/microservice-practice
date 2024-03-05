using PaymentAuthorization.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public interface IPaymentAuthorizationProcessor
    {
        PaymentAuthorizationResponse ProcessAuthorization(PaymentAuthorizationRequest paymentAuthorization);
    }
}
