using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(_ => _.SaleId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(_ => _.ProductId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(_ => _.ProductName)
            .IsRequired();
        
        builder.Property(_ => _.ProductDescription)
            .IsRequired(false);

        builder.Property(_ => _.Quantity)
            .IsRequired();

        builder.Property(_ => _.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(_ => _.Discount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(_ => _.TotalAmountItem)
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

        builder.HasOne(_ => _.Sale)
            .WithMany(_ => _.Items)
            .HasForeignKey(_ => _.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
