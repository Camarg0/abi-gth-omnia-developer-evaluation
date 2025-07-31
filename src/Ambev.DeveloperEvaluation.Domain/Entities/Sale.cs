using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
        Status = SaleStatus.Active;
    }

    public void Cancel()
    {
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = SaleStatus.Cancelled;

        foreach (var item in Items.Where(x => x.Status == SaleItemStatus.Active))
        {
            item.Cancel();
        }
    }

    public void AddItem(SaleItem saleItem)
    {
        var validationResult = saleItem.Validate();

        if (!validationResult.IsValid)
            throw new ArgumentException(string.Join("\n", validationResult.Errors.Select(e => $"{e.Error}: {e.Detail}")));

        var existingItem = Items.FirstOrDefault(x => x.ProductId == saleItem.ProductId && x.Status == SaleItemStatus.Active);

        if (existingItem != null) // If the item exists already
        {
            if (existingItem.Quantity + saleItem.Quantity > 20)
                throw new InvalidOperationException("Cannot exceed the maximum of 20 items per product");

            existingItem.Quantity += saleItem.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
            existingItem.CalculateDiscount();
            existingItem.CalculateTotalAmountItem();
        }
        else
        {
            saleItem.SaleId = Id;
            saleItem.CalculateDiscount();
            saleItem.CalculateTotalAmountItem();
            Items.Add(saleItem);
        }

        // In the end I have to recalculate the total amount of the sale
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateItem(Guid itemId, string productName, string productDescription, int quantity, decimal unitPrice)
    {
        var existingItem = Items.FirstOrDefault(x => x.Id == itemId && x.Status == SaleItemStatus.Active);

        if (existingItem == null)
            throw new ArgumentException($"Sale item with Id {itemId} not found or not active");

        existingItem.ProductName = productName;
        existingItem.ProductDescription = productDescription;
        existingItem.Quantity = quantity;
        existingItem.UnitPrice = unitPrice;
        existingItem.UpdatedAt = DateTime.UtcNow;
        existingItem.CalculateDiscount();
        existingItem.CalculateTotalAmountItem();

        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid itemId)
    {
        var existingItem = Items.FirstOrDefault(x => x.Id == itemId);
        if (existingItem == null)
            throw new ArgumentException("There is no item with this id in the sale");

        existingItem.Cancel();

        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearItems()
    {
        if (Status != SaleStatus.Active)
            throw new InvalidOperationException("The sale must be Active to be have its items cleared");

        Items.Clear();
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    // sum of all items
    public void CalculateTotalAmount()
    {
        TotalAmount = Items
        .Where(x => x.Status == SaleItemStatus.Active)
        .Sum(x => x.TotalAmountItem);
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}