using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using FluentValidation;

public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
    public DeleteSaleValidator()
    {
        RuleFor(sale => sale.Id)
        .NotEmpty()
        .WithMessage("Sale must have an id");
    }
}