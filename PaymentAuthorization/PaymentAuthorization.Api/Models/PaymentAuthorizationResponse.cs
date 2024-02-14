using PaymentAuthorization.Api.Models.Enums;

namespace PaymentAuthorization.Api.Models
{
    public class PaymentAuthorizationResponse
    {
        public int Id { get; set; }
        public AuthorizationStatusEnum Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
