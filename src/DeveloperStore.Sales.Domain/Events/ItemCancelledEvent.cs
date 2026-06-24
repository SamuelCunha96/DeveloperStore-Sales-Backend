using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Domain.Events;

public sealed record ItemCancelledEvent(
    Guid SaleId,
    Guid ItemId,
    Guid ProductId,
    string ProductName) : IDomainEvent;
