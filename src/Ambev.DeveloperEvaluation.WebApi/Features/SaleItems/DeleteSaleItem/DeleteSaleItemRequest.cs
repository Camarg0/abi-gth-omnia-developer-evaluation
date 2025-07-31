using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.DeleteSaleItem;

public class DeleteSaleItemRequest
{
    [Required]
    public Guid SaleId { get; set; }

    [Required]
    public Guid ItemId { get; set; }
}
