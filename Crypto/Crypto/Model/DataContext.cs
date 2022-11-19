using Crypto.Model.Entities;
using Microsoft.EntityFrameworkCore;


namespace Crypto.Model
{
    internal class DataContext : DbContext
    {
        private string ConnectionString;
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<PurchaseTrade> PurchaseTrade { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleTrade> SaleTrade { get; set; }
        public DataContext(string connectionString) :base ()
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToLower());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnBaseName().ToLower());
                }
            }

            builder.Entity<PurchaseTrade>()
                .HasOne(x => x.Purchase).WithMany(x => x.Trades).HasForeignKey(x => x.PurchaseId);
            builder.Entity<SaleTrade>()
                .HasOne(x => x.Sale).WithMany(x => x.Trades).HasForeignKey(x => x.SaleId);

        }
    }
}
           
        

