using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.EventHandlers;

public class ItemCancelledEventHandler : INotificationHandler<DomainEventNotification<ItemCancelledEvent>>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ItemCancelledEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        _logger.LogInformation(
            "Item cancelled: {ProductName} (ItemId: {ItemId}) from sale {SaleId}",
            e.ProductName, e.ItemId, e.SaleId);

        return Task.CompletedTask;
    }
}
