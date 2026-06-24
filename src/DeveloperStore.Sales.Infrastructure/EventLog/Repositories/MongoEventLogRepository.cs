using System.Text.Json;
using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Infrastructure.EventLog.Documents;
using MongoDB.Driver;

namespace DeveloperStore.Sales.Infrastructure.EventLog.Repositories;

public class MongoEventLogRepository : IMongoEventLogRepository
{
    private readonly IMongoCollection<EventLogDocument> _collection;

    public MongoEventLogRepository(IMongoDatabase database)
        => _collection = database.GetCollection<EventLogDocument>("event_logs");

    public Task LogAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var saleId = domainEvent.GetType().GetProperty("SaleId")?.GetValue(domainEvent) is Guid id
            ? id
            : Guid.Empty;

        var doc = new EventLogDocument
        {
            EventType = domainEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            OccurredAt = DateTime.UtcNow,
            SaleId = saleId
        };

        return _collection.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }
}
