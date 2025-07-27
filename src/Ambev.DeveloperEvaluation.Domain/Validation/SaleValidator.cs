using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
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

        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item")
            .Must(items => items.Any(i => i.Status == SaleItemStatus.Active))
            .WithMessage("Sale must have at least one active item")
            .When(sale => sale.Items != null);

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator())
            .When(sale => sale.Items != null && sale.Items.Any());

        RuleFor(sale => sale.UpdatedAt)
            .GreaterThanOrEqualTo(sale => sale.CreatedAt)
            .WithMessage("Updated date cannot be before created date")
            .When(sale => sale.UpdatedAt.HasValue);

        RuleFor(sale => sale.CancelledAt)
            .GreaterThanOrEqualTo(sale => sale.CreatedAt)
            .WithMessage("Cancelled date cannot be before created date")
            .When(sale => sale.CancelledAt.HasValue);
    }
}