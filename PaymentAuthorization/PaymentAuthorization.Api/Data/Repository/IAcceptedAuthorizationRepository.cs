using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.Data.Repository
{
    public interface IAcceptedAuthorizationRepository
    {
        Task SaveAcceptedAuthorization(AcceptedPaymentAuthorization acceptedAuthorization);
        Task<IEnumerable<AcceptedPaymentAuthorization>> GetAllAcceptedAuthorizations();
        Task<AcceptedPaymentAuthorization?> GetAcceptedAuthorizationById(int id);
        Task DeleteAcceptedAuthorization(int id);
        Task UpdateAcceptedAuthorization(AcceptedPaymentAuthorization acceptedAuthorization);

    }
}
