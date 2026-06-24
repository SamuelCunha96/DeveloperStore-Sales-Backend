using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Domain.Events;

public sealed record SaleCreatedEvent(
    Guid SaleId,
    string SaleNumber,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    DateTime SaleDate,
    decimal TotalAmount) : IDomainEvent;
