using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

public class DeleteSaleItemValidator : AbstractValidator<DeleteSaleItemCommand>
{
    public DeleteSaleItemValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID must be provided");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID must be provided");
    }
}
