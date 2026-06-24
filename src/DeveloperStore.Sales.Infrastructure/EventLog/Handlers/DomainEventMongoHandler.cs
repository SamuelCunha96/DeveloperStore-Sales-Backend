using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Infrastructure.EventLog.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Infrastructure.EventLog.Handlers;

public class DomainEventMongoHandler<TEvent> : INotificationHandler<DomainEventNotification<TEvent>>
    where TEvent : IDomainEvent
{
    private readonly IMongoEventLogRepository _repository;

    public DomainEventMongoHandler(IMongoEventLogRepository repository) => _repository = repository;

    public Task Handle(DomainEventNotification<TEvent> notification, CancellationToken cancellationToken)
        => _repository.LogAsync(notification.DomainEvent, cancellationToken);
}
