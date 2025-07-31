using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.DeleteSaleItem;

public class DeleteSaleItemRequestValidator : AbstractValidator<DeleteSaleItemRequest>
{
    public DeleteSaleItemRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID must not be empty");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID must not be empty");
    }
}
