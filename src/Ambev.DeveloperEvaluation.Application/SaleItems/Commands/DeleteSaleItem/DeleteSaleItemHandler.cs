using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

public class DeleteSaleItemHandler : IRequestHandler<DeleteSaleItemCommand, DeleteSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleItemHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<DeleteSaleItemResult> Handle(DeleteSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleItemValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} was not found");

        if (existingSale.Status != Domain.Enums.SaleStatus.Active)
            throw new InvalidOperationException("You can only delete items from an active sale");

        existingSale.RemoveItem(command.ItemId);

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        return new DeleteSaleItemResult
        {
            ItemId = command.ItemId
        };
    }
}