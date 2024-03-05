using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentAuthorization.Api.Models
{
    public class AcceptedPaymentAuthorization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ClientId { get; set; }   
        public DateTime AcceptedDate { get; set; }
    }
}
