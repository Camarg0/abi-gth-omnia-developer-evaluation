using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdRequestValidator : AbstractValidator<GetSalesByBranchIdRequest>
{
    public GetSalesByBranchIdRequestValidator()
    {
        RuleFor(sales => sales.BranchId)
            .NotEmpty()
            .WithMessage("Branch must have an id");
    }
}
