using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdResult
{
    public Guid BranchId { get; set; }
    public List<BranchSaleResult> Sales { get; set; } = new();
    public int TotalSales { get; set; }
    public decimal TotalAmount { get; set; }
}

public class BranchSaleResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public SaleStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<BranchSaleItemResult> Items { get; set; } = new();
}

public class BranchSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}
