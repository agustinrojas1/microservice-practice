using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentAuthorization.Api.Data.Context;
using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.Data.Repository
{
    public class AcceptedAuthorizationRepository : IAcceptedAuthorizationRepository
    {
        private readonly AuthorizationDbContext _dbContext;
        private readonly ILogger<AcceptedAuthorizationRepository> _logger;

        public AcceptedAuthorizationRepository(AuthorizationDbContext dbContext, ILogger<AcceptedAuthorizationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task DeleteAcceptedAuthorization(int id)
        {
            var acceptedAuthorization = await _dbContext.AcceptedPaymentAuthorizations.FindAsync(id);
            if (acceptedAuthorization != null)
            {
                _logger.LogInformation("Deleting accepted authorization: " + JsonConvert.SerializeObject(acceptedAuthorization));
                _dbContext.AcceptedPaymentAuthorizations.Remove(acceptedAuthorization);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<AcceptedPaymentAuthorization?> GetAcceptedAuthorizationById(int id)
        {
            return await _dbContext.AcceptedPaymentAuthorizations.FindAsync(id);
        }

        public async Task<IEnumerable<AcceptedPaymentAuthorization>> GetAllAcceptedAuthorizations()
        {
            return await _dbContext.AcceptedPaymentAuthorizations.ToListAsync();
        }

        public async Task SaveAcceptedAuthorization(AcceptedPaymentAuthorization acceptedAuthorization)
        {
            _logger.LogInformation("Saving accepted authorization: " + JsonConvert.SerializeObject(acceptedAuthorization));

            await _dbContext.AcceptedPaymentAuthorizations.AddAsync(acceptedAuthorization);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAcceptedAuthorization(AcceptedPaymentAuthorization acceptedAuthorization)
        {
            _logger.LogInformation("updating accepted authorization: " + JsonConvert.SerializeObject(acceptedAuthorization));

            _dbContext.AcceptedPaymentAuthorizations.Update(acceptedAuthorization);
            await _dbContext.SaveChangesAsync();
        }
    }
}
