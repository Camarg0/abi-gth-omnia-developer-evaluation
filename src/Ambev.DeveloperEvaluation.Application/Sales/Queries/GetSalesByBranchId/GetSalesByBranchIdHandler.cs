using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;

public class GetSalesByBranchIdHandler : IRequestHandler<GetSalesByBranchIdQuery, GetSalesByBranchIdResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesByBranchIdHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<GetSalesByBranchIdResult> Handle(GetSalesByBranchIdQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetSalesByBranchIdValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var sales = await _saleRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);

        var salesList = sales.ToList();
        var mappedSales = _mapper.Map<List<BranchSaleResult>>(salesList);

        return new GetSalesByBranchIdResult
        {
            BranchId = request.BranchId,
            Sales = mappedSales,
            TotalSales = salesList.Count,
            TotalAmount = salesList.Sum(s => s.TotalAmount)
        };
    }
}
