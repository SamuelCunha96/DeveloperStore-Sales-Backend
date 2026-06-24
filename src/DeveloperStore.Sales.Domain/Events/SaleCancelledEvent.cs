using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Domain.Events;

public sealed record SaleCancelledEvent(
    Guid SaleId,
    string SaleNumber) : IDomainEvent;
