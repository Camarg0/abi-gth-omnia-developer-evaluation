using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmountItem { get; set; }
    public SaleItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    public SaleItem()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleItemStatus.Active;
    }

    public void Cancel()
    {
        Status = SaleItemStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CalculateDiscount()
    {
        if (Quantity >= 10 && Quantity <= 20)
            Discount = UnitPrice * Quantity * 0.20m;
        else if (Quantity >= 4 && Quantity < 10)
            Discount = UnitPrice * Quantity * 0.10m;
        else
            Discount = 0;
    }

    public void CalculateTotalAmountItem()
    {
        TotalAmountItem = (Quantity * UnitPrice) - Discount;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}