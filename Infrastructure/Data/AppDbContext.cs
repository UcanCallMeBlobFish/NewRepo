using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.ICNumber).IsUnique(false);
                e.HasIndex(u => u.PhoneNumber).IsUnique(false);
                e.HasIndex(u => u.Email).IsUnique(false);
            });
        }
    }
}
