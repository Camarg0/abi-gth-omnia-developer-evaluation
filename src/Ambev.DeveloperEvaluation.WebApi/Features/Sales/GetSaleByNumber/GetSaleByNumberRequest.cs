using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleByNumber;

public class GetSaleByNumberRequest
{
    [Required]
    public string SaleNumber { get; set; } = string.Empty;
}
