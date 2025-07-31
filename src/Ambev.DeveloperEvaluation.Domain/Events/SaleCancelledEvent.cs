using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleCancelledEvent : INotification
{
    public Guid Id { get; }

    public SaleCancelledEvent(Guid saleId)
    {
        Id = saleId;
    }
}