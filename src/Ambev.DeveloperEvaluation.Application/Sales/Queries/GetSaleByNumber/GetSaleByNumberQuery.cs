using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

public class GetSaleByNumberQuery : IRequest<GetSaleByNumberResult>
{
    public string SaleNumber { get; set; } = string.Empty;

    public GetSaleByNumberQuery(string saleNumber)
    {
        SaleNumber = saleNumber;
    }
}
