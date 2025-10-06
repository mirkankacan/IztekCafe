using IztekCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IztekCafe.Persistance.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.OrderId).IsRequired();

            builder.HasOne(p => p.Order)
                .WithOne(p => p.Payment)
                .HasForeignKey<Payment>(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}