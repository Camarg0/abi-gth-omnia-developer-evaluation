using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomerId;

public class GetSalesByCustomerIdResponse
{
    public Guid CustomerId { get; set; }
    public List<CustomerSaleResponse> Sales { get; set; } = new();
    public int TotalSales { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CustomerSaleResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public SaleStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CustomerSaleItemResponse> Items { get; set; } = new();
}

public class CustomerSaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}
