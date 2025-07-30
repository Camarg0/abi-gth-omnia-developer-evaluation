using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

public class GetSaleByNumberValidator : AbstractValidator<GetSaleByNumberQuery>
{
    public GetSaleByNumberValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale must have a number");
    }
}
