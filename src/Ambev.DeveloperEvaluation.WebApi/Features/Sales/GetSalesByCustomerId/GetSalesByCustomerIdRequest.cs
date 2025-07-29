using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomerId;

public class GetSalesByCustomerIdRequest
{
    [Required]
    public Guid CustomerId { get; set; }
}
