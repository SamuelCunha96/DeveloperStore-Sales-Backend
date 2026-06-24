using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.EventHandlers;

public class SaleCreatedEventHandler : INotificationHandler<DomainEventNotification<SaleCreatedEvent>>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<SaleCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        _logger.LogInformation(
            "Sale created: {SaleNumber} | Customer: {CustomerName} | Branch: {BranchName} | Total: {TotalAmount:C}",
            e.SaleNumber, e.CustomerName, e.BranchName, e.TotalAmount);

        return Task.CompletedTask;
    }
}
