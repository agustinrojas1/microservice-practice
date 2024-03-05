using Microsoft.EntityFrameworkCore;
using PaymentAuthorization.Api.Models;

namespace PaymentAuthorization.Api.Data.Context
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcceptedPaymentAuthorization>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<PaymentAuthorizationRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        public override int SaveChanges()
        {
            SetCreatedDate();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedDate()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is PaymentAuthorizationRequest paymentRequest)
                {
                    paymentRequest.CreatedDate = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Added && entry.Entity is AcceptedPaymentAuthorization acceptedPayment)
                {
                    acceptedPayment.AcceptedDate = DateTime.UtcNow;
                }
            }
        }

        public DbSet<PaymentAuthorizationRequest> PaymentAuthorizationRequests { get; set; }
        public DbSet<AcceptedPaymentAuthorization> AcceptedPaymentAuthorizations { get; set; }

    }
}
