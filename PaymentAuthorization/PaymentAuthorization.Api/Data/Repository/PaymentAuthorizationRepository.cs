using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentAuthorization.Api.Data.Context;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Dtos;

namespace PaymentAuthorization.Api.Data.Repository
{
    public class PaymentAuthorizationRepository : IPaymentAuthorizationRepository
    {
        private readonly AuthorizationDbContext _dbContext;
        private readonly ILogger<PaymentAuthorizationRepository> _logger;

        public PaymentAuthorizationRepository(AuthorizationDbContext dbContext, ILogger<PaymentAuthorizationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task DeleteAuthorization(int id)
        {
            var authorization = await _dbContext.PaymentAuthorizationRequests.FindAsync(id);
            if (authorization != null)
            {
                _dbContext.PaymentAuthorizationRequests.Remove(authorization);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Authorization with id #{id} not found");
            }
        }

        public async Task<IEnumerable<PaymentAuthorizationRequest>> GetAllAuthorizations()
        {
            return await _dbContext.PaymentAuthorizationRequests.ToListAsync();
        }

        public async Task<PaymentAuthorizationRequest?> GetAuthorizationById(int id)
        {
            return await _dbContext.PaymentAuthorizationRequests.FindAsync(id);
        }

        public async Task SaveAuthorization(PaymentAuthorizationRequest authorizationRequest)
        {
            _logger.LogInformation("Saving authorization: " + JsonConvert.SerializeObject(authorizationRequest));
            try
            {
                await _dbContext.PaymentAuthorizationRequests.AddAsync(authorizationRequest);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex}");

            }
        }

        public async Task UpdateAuthorization(PaymentAuthorizationRequest authorizationRequest)
        {
            _logger.LogInformation("Updating authorization: " + JsonConvert.SerializeObject(authorizationRequest));
            _dbContext.PaymentAuthorizationRequests.Update(authorizationRequest);
            await _dbContext.SaveChangesAsync();
        }


    }
}
