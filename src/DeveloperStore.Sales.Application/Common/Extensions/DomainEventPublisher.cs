using DeveloperStore.Sales.Domain.Common;
using MediatR;

namespace DeveloperStore.Sales.Application.Common.Extensions;

public static class DomainEventPublisher
{
    public static async Task PublishDomainEventsAsync(
        this IPublisher publisher,
        Entity entity,
        CancellationToken cancellationToken = default)
    {
        var events = entity.DomainEvents.ToList();
        entity.ClearDomainEvents();

        foreach (var domainEvent in events)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(notificationType, domainEvent) as INotification;
            if (notification is not null)
                await publisher.Publish(notification, cancellationToken);
        }
    }
}
