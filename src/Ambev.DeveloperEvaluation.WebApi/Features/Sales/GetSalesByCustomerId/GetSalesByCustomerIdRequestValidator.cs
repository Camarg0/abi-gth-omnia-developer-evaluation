using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomerId;

public class GetSalesByCustomerIdRequestValidator : AbstractValidator<GetSalesByCustomerIdRequest>
{
    public GetSalesByCustomerIdRequestValidator()
    {
        RuleFor(sales => sales.CustomerId)
            .NotEmpty()
            .WithMessage("Customer must have an id");
    }
}
