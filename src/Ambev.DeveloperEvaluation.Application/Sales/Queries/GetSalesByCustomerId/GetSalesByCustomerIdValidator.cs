using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomerId;

public class GetSalesByCustomerIdValidator : AbstractValidator<GetSalesByCustomerIdQuery>
{
    public GetSalesByCustomerIdValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer must have an id");
    }
}
