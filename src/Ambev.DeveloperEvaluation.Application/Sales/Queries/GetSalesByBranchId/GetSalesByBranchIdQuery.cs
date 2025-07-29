using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdQuery : IRequest<GetSalesByBranchIdResult>
{
    public Guid BranchId { get; set; }
    
    public GetSalesByBranchIdQuery(Guid branchId)
    {
        BranchId = branchId;
    }
}
