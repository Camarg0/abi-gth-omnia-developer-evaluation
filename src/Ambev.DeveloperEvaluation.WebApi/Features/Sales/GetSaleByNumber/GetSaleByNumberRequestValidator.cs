using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleByNumber;

public class GetSaleByNumberRequestValidator : AbstractValidator<GetSaleByNumberRequest>
{
    public GetSaleByNumberRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale must have a number");
    }
}
