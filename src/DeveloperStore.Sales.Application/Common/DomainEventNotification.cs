using DeveloperStore.Sales.Domain.Common;
using MediatR;

namespace DeveloperStore.Sales.Application.Common;

public record DomainEventNotification<TEvent>(TEvent DomainEvent) : INotification
    where TEvent : IDomainEvent;
