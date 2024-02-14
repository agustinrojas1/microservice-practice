namespace PaymentAuthorization.Api.Models
{
    public class PaymentAuthorizationRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Amount { get; set; }
        public DateTime CreatedDate { get; set; }        

    }
}
