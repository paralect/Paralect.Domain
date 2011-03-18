using MongoDB.Bson;

namespace Paralect.Domain.EventStore.Providers.MongoDB.Serialization
{
    public static class CommandMetadataSerializer
    {
        public static CommandMetadata Deserialize(BsonDocument doc)
        {
            var eventMetadata = new CommandMetadata{
                CommandId = doc.GetString("CommandId"),
                UserId = doc.GetString("UserId"),
                CreatedDate = doc.GetDateTime("CreatedDate"),
                TypeName = doc.GetString("TypeName")
            };

            return eventMetadata;
        }
    }
}