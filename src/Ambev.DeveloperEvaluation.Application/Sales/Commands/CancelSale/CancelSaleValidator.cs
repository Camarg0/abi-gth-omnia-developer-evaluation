using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using FluentValidation;

public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleValidator()
    {
        RuleFor(sale => sale.Id)
        .NotEmpty()
        .WithMessage("Sale must have an id");
    }
}