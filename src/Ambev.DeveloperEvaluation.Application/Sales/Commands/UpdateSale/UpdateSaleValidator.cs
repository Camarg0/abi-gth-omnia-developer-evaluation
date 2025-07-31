using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Sale must have an id");

        RuleFor(command => command.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(command => command.Items)
            .SetValidator(new UpdateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for UpdateSaleItemCommand
/// </summary>
public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    public UpdateSaleItemCommandValidator()
    {
        RuleFor(item => item.Id)
            .NotEmpty()
            .WithMessage("Sale item must have an id");

        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product must have an id");

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .WithMessage("Product must have a name");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot exceed the maximum of 20 items per product");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0.");
    }
}