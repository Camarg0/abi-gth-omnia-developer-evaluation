using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty()
            .WithMessage("Sale must have an id");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new UpdateSaleItemSaleRequestValidator())
            .When(sale => sale.Items != null && sale.Items.Any());
    }
}

public class UpdateSaleItemSaleRequestValidator : AbstractValidator<UpdateSaleItemSaleRequest>
{
    public UpdateSaleItemSaleRequestValidator()
    {
        RuleFor(saleItem => saleItem.Id)
            .NotEmpty()
            .WithMessage("Sale item must have an id");

        RuleFor(saleItem => saleItem.Quantity)
            .LessThanOrEqualTo(20).WithMessage("The maximum limit is 20 items per product")
            .GreaterThan(0).WithMessage("The number of items must be more than 0");

        RuleFor(saleItem => saleItem.UnitPrice)
            .GreaterThan(0)
            .WithMessage("The price of the item cannot be zero");

        RuleFor(saleItem => saleItem.ProductId)
            .NotEmpty()
            .WithMessage("The product must have an id");

        RuleFor(saleItem => saleItem.ProductName)
            .NotEmpty()
            .WithName("The product must have a name");
    }
}