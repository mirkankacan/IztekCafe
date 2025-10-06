using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Data.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<Stock> Stocks => Set<Stock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistanceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IBaseEntity>();

            foreach (var entry in entries)
            {
                dynamic entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.Now;
                        entry.Property(p => p.CreatedAt).IsModified = false;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}