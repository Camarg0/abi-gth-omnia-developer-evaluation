using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdValidator : AbstractValidator<GetSalesByBranchIdQuery>
{
    public GetSalesByBranchIdValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch must have an id");
    }
}
