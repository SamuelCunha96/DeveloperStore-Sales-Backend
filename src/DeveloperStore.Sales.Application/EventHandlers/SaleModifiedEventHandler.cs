using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.EventHandlers;

public class SaleModifiedEventHandler : INotificationHandler<DomainEventNotification<SaleModifiedEvent>>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<SaleModifiedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        _logger.LogInformation(
            "Sale modified: {SaleNumber} | New total: {TotalAmount:C}",
            e.SaleNumber, e.TotalAmount);

        return Task.CompletedTask;
    }
}
