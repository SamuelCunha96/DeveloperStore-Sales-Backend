using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Infrastructure.EventLog.Repositories;

public interface IMongoEventLogRepository
{
    Task LogAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
