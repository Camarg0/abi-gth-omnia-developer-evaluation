using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
        }

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
        {
            throw new InvalidOperationException($"The sale you are trying to delete does not exist");
        }

        var success = await _saleRepository.DeleteAsync(command.Id, cancellationToken);

        if (!success)
        {
            throw new InvalidOperationException($"An error occurred while deleting the sale with id {command.Id}");
        }

        return new DeleteSaleResponse
        {
            Success = true
        };
    }
}
