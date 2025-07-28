using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(_ => _.SaleNumber)
            .IsRequired();

        builder.Property(_ => _.SaleDate)
            .IsRequired();

        builder.Property(_ => _.CustomerId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(_ => _.CustomerName)
            .IsRequired();

        builder.Property(_ => _.CustomerEmail)
            .IsRequired(false);

        builder.Property(_ => _.BranchId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(_ => _.BranchName)
            .IsRequired();

        builder.Property(_ => _.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(_ => _.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(_ => _.CreatedAt)
            .IsRequired();

        builder.Property(_ => _.UpdatedAt)
            .IsRequired(false);

        builder.Property(_ => _.CancelledAt)
            .IsRequired(false);

        builder.HasMany(_ => _.Items)
            .WithOne(_ => _.Sale)
            .HasForeignKey(_ => _.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(_ => _.SaleNumber)
            .IsUnique();
    }
}
