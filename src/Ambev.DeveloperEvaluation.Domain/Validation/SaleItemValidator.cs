using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(saleItem => saleItem.Quantity)
            .LessThanOrEqualTo(20).WithMessage("The maximum limit is 20 items per product")
            .GreaterThan(0).WithMessage("The number of items must be more than 0");

        RuleFor(saleItem => saleItem.UnitPrice)
            .GreaterThan(0)
            .WithMessage("The price of the item cannot be zero");

        RuleFor(saleItem => saleItem.ProductId)
            .NotEmpty()
            .WithName("The product must have an id");
        
        RuleFor(saleItem => saleItem.ProductDescription)
            .NotEmpty()
            .WithName("The product must have a description");
    }
}