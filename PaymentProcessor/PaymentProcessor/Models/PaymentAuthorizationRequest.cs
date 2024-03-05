using PaymentAuthorization.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentAuthorization.Api.Models
{
    public class PaymentAuthorizationRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Amount { get; set; }
        public AuthorizationTypeEnum AuthorizationTypeEnum { get; set; }
        public AuthorizationStatusEnum Status { get; set; }
        public ClientTypeEnum ClientType { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}