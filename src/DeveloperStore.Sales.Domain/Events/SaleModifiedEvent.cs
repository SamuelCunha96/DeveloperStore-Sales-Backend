using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Domain.Events;

public sealed record SaleModifiedEvent(
    Guid SaleId,
    string SaleNumber,
    decimal TotalAmount) : IDomainEvent;
