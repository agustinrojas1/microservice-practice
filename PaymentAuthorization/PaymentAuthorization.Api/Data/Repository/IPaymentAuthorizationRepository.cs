using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Dtos;

namespace PaymentAuthorization.Api.Data.Repository
{
    public interface IPaymentAuthorizationRepository
    {
        Task SaveAuthorization(PaymentAuthorizationRequest authorizationRequest);
        Task<IEnumerable<PaymentAuthorizationRequest>> GetAllAuthorizations();
        Task<PaymentAuthorizationRequest?> GetAuthorizationById(int id);
        Task DeleteAuthorization(int id);
        Task UpdateAuthorization(PaymentAuthorizationRequest authorizationRequest);

    }
}
