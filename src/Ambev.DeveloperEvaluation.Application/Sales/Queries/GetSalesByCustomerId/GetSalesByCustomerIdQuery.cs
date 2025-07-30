using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomerId;

public class GetSalesByCustomerIdQuery : IRequest<GetSalesByCustomerIdResult>
{
    public Guid CustomerId { get; set; }

    public GetSalesByCustomerIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}
