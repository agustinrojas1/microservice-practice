using PaymentAuthorization.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentAuthorization.Api.Models
{
    public class PaymentAuthorizationResponse
    {
        public int AuthorizationRequestId { get; set; }
        public AuthorizationStatusEnum Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
