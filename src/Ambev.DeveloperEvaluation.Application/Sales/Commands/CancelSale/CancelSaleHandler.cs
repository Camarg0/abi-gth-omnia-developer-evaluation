using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;

    public CancelSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
        {
            throw new KeyNotFoundException($"The sale you are trying to cancel does not exist");
        }

        if (existingSale.Status == Domain.Enums.SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Sale is already cancelled");
        }

        var success = await _saleRepository.CancelAsync(existingSale, cancellationToken);

        if (!success)
        {
            throw new InvalidOperationException($"An error occurred while cancelling the sale with ID {command.Id}");
        }

        return new CancelSaleResult
        {
            Id = command.Id
        };
    }
}
