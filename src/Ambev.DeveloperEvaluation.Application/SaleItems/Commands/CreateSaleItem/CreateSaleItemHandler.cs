using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

public class CreateSaleItemHandler : IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CreateSaleItemHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validation = command.Validate();
        if (!validation.IsValid)
            throw new ValidationException(string.Join("; ", validation.Errors.Select(e => $"{e.Error}: {e.Detail}")));

        var existingSale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID '{command.SaleId}' not found");

        if (existingSale.Status != Domain.Enums.SaleStatus.Active)
            throw new InvalidOperationException("Cannot add items to a sale that is not Active");

        var saleItem = _mapper.Map<SaleItem>(command);
        existingSale.AddItem(saleItem);

        var saleValidation = existingSale.Validate();
        if (!saleValidation.IsValid)
            throw new ValidationException(string.Join("; ", saleValidation.Errors.Select(e => e.Error)));

        await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        return _mapper.Map<CreateSaleItemResult>(saleItem);
    }
}
