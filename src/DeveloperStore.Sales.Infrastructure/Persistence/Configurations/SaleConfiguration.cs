using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.TotalAmount)
            .HasPrecision(18, 2);

        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
