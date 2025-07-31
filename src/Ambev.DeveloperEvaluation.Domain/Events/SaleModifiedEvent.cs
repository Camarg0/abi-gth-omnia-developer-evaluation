using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleModifiedEvent : INotification
{
    public Guid Id { get; }

    public SaleModifiedEvent(Guid saleId)
    {
        Id = saleId;
    }
}
