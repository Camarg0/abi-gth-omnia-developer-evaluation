using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (existingSale.Status != Domain.Enums.SaleStatus.Active)
            throw new InvalidOperationException($"The sale you're trying to update is Cancelled, and it must be Active");

        // the addItem method already covers update existing items and adding new ones
        foreach (var itemCommand in command.Items)
        {
            var existingItem = existingSale.Items
                .FirstOrDefault(_ => _.Id == itemCommand.Id);

            if (existingItem != null)
            {
                existingSale.UpdateItem(
                    itemCommand.Id,
                    itemCommand.ProductName,
                    itemCommand.ProductDescription,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice
                );
            }
            else
            {
                var newItem = new SaleItem
                {
                    ProductId = itemCommand.ProductId,
                    ProductName = itemCommand.ProductName,
                    ProductDescription = itemCommand.ProductDescription,
                    Quantity = itemCommand.Quantity,
                    UnitPrice = itemCommand.UnitPrice
                };

                existingSale.AddItem(newItem);
            }
        }

        // if I have any items removed in the new updated sale
        var itemsToRemove = existingSale.Items
            .Where(existingItem => !command.Items.Any(commandItem => commandItem.ProductId == existingItem.ProductId))
            .ToList();

        foreach (var itemToRemove in itemsToRemove)
        {
            existingSale.RemoveItem(itemToRemove.Id);
        }

        // validate the updated sales
        var saleValidation = existingSale.Validate();
        if (!saleValidation.IsValid)
            throw new ValidationException(string.Join("; ", saleValidation.Errors.Select(e => e.Error)));

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        await _mediator.Publish(new SaleModifiedEvent(updatedSale.Id), cancellationToken);

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}