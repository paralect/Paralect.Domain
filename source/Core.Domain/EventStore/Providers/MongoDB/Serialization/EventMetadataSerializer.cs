using MongoDB.Bson;

namespace Core.Domain.EventStore.Providers.MongoDB.Serialization
{
    public static class EventMetadataSerializer
    {
        public static IEventMetadata FromBson(BsonDocument doc)
        {
            var eventMetadata = new EventMetadata()
            {
                CommandId = doc.GetString("CommandId"),
                EventId = doc.GetString("EventId"),
                UserId = doc.GetString("UserId"),
                StoredDate = doc.GetDateTime("StoredDate"),
                TypeName = doc.GetString("TypeName"),
            };

            return eventMetadata;
        }
    }
}
