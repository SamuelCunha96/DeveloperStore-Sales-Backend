using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.EventHandlers;

public class SaleCancelledEventHandler : INotificationHandler<DomainEventNotification<SaleCancelledEvent>>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<SaleCancelledEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        _logger.LogInformation("Sale cancelled: {SaleNumber}", e.SaleNumber);

        return Task.CompletedTask;
    }
}
