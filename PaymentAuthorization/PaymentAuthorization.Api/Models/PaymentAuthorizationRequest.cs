using PaymentAuthorization.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PaymentAuthorization.Api.Models
{
    public class PaymentAuthorizationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Amount { get; set; }
        public AuthorizationTypeEnum AuthorizationType { get; set; } 
        public AuthorizationStatusEnum Status { get; set; } = AuthorizationStatusEnum.Pending;
        public ClientTypeEnum ClientType { get; set; }
        public DateTime CreatedDate { get; set; }        
    }
}
