using IztekCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IztekCafe.Persistance.Data.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).UseIdentityColumn();
            builder.Property(s => s.ProductId).IsRequired();
            builder.Property(s => s.Quantity).IsRequired();
            builder.Property(s => s.Unit).IsRequired().HasMaxLength(20);
            builder.Ignore(s => s.HasStock);

            builder.HasOne(s => s.Product)
               .WithOne(p => p.Stock)
               .HasForeignKey<Stock>(s => s.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}