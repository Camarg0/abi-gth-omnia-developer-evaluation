using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdRequest
{
    [Required]
    public Guid BranchId { get; set; }
}
