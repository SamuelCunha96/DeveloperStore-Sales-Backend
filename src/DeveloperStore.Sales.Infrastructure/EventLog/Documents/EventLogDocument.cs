using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeveloperStore.Sales.Infrastructure.EventLog.Documents;

public class EventLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public Guid SaleId { get; set; }
}
