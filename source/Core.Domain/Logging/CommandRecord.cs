using Acropolis.Foundation.Infrastructure.MongoServers;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class CommandRecord
    {
        public BsonDocument CommandDocument { get; set; }
        public CommandMetadata Metadata { get; set; }
        public CommandHandlerRecordCollection Handlers { get; set; }

        public static CommandRecord FromBson(BsonDocument doc)
        {
            var commandDocument = doc.GetBsonDocument("Command");

            var record = new CommandRecord
            {
                CommandDocument = commandDocument,
                Metadata = CommandMetadata.FromBson(commandDocument.GetBsonDocument("Metadata")),
                Handlers = CommandHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }        
    }
}