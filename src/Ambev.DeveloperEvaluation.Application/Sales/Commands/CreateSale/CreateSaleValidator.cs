using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale must have a number");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale must have a date")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer must have an id");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer must have a name");

        RuleFor(sale => sale.CustomerEmail)
            .SetValidator(new EmailValidator())
            .When(sale => !string.IsNullOrEmpty(sale.CustomerEmail));

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch must have an id");

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .WithMessage("Branch must have a name");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new CreateSaleItemValidator())
            .When(sale => sale.Items != null && sale.Items.Any());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemCommandRequest>
{
    public CreateSaleItemValidator()
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

        RuleFor(saleItem => saleItem.ProductName)
            .NotEmpty()
            .WithName("The product must have a name");
        
        RuleFor(saleItem => saleItem.ProductDescription)
            .NotEmpty()
            .WithName("The product must have a description");
    }
}
