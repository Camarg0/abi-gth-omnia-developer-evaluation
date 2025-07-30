using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomerId;

/// <summary>
/// Handler for processing GetSalesByCustomerIdQuery requests
/// </summary>
public class GetSalesByCustomerIdHandler : IRequestHandler<GetSalesByCustomerIdQuery, GetSalesByCustomerIdResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    
    public GetSalesByCustomerIdHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<GetSalesByCustomerIdResult> Handle(GetSalesByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetSalesByCustomerIdValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var sales = await _saleRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        var salesList = sales.ToList();
        var mappedSales = _mapper.Map<List<CustomerSaleResult>>(salesList);

        return new GetSalesByCustomerIdResult
        {
            CustomerId = request.CustomerId,
            Sales = mappedSales,
            TotalSales = salesList.Count,
            TotalAmount = salesList.Sum(s => s.TotalAmount)
        };
    }
}
