using Microsoft.EntityFrameworkCore;
using TesInspiro.Domain;
using TesInspiro.Helper;

namespace TesInspiro.Data
{
    public class TesInspiroDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string environment = AppConfig.Config.Environment.ToUpper();
            if (!string.IsNullOrEmpty(environment) && environment == "DEV")
            {
                string connectionString = (environment == "DEV") ?
                                           AppConfig.Config.ConfigDev.connection :
                                           string.Empty;

                if (!string.IsNullOrEmpty(connectionString))
                    optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MsItems>()
                .HasKey(s => s.id);
            modelBuilder.Entity<ShoppingCart>()
                .HasKey(s => s.id);
            modelBuilder.Entity<Payment>()
                .HasKey(s => s.id);
        }

        #region DbSet
        public DbSet<MsItems>? MsItems { get; set; }
        public DbSet<ShoppingCart>? ShoppingCarts { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        #endregion
    }
}