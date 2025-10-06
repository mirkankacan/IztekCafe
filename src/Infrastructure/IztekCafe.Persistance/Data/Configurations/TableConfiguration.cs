using IztekCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IztekCafe.Persistance.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(20);

            builder.HasMany(t => t.Orders)
                .WithOne(o => o.Table)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}