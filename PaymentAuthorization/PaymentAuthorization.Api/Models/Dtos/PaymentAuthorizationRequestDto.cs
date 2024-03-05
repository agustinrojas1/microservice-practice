using PaymentAuthorization.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentAuthorization.Api.Models.Dtos
{
    public class PaymentAuthorizationRequestDto
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        [EnumDataType(typeof(AuthorizationTypeEnum))]
        public AuthorizationTypeEnum AuthorizationType { get; set; }
        [Required]
        [EnumDataType(typeof(ClientTypeEnum))]
        public ClientTypeEnum ClientType { get; set; }
    }
}
