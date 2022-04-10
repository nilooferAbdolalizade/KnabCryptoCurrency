using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class CryptoCurrencyContext : DbContext
    {
        public CryptoCurrencyContext(DbContextOptions<CryptoCurrencyContext> options)
            : base(options)
        {
        }

        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CryptoCurrency>().HasKey(x => x.Id);
            modelBuilder.Entity<CryptoCurrency>().Ignore(x => x.CurrencyQuotes);

            base.OnModelCreating(modelBuilder);
        }
    }
}
