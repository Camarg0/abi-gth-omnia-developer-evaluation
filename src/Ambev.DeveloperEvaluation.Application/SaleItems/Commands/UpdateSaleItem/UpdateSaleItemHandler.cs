using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public UpdateSaleItemHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validation = command.Validate();
        if (!validation.IsValid)
            throw new ValidationException(string.Join("; ", validation.Errors.Select(e => $"{e.Error}: {e.Detail}")));

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID '{command.SaleId}' not found");

        if (sale.Status != SaleStatus.Active)
            throw new InvalidOperationException("Cannot update item in a cancelled sale.");

        sale.UpdateItem(
            command.Id,
            command.ProductName,
            command.ProductDescription,
            command.Quantity,
            command.UnitPrice
        );

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var updatedItem = updatedSale.Items.First(x => x.Id == command.Id);
        return _mapper.Map<UpdateSaleItemResult>(updatedItem);
    }
}
