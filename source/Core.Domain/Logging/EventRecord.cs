using System.Collections.Generic;
using Acropolis.Foundation.Infrastructure.MongoServers;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class EventRecord
    {
        public BsonDocument EventDocument { get; set; }
        public EventMetadata Metadata { get; set; }
        public EventHandlerRecordCollection Handlers { get; set; }

        public static EventRecord FromBson(BsonDocument doc)
        {
            var eventDocument = doc.GetBsonDocument("Event");
            var record = new EventRecord()
            {
                EventDocument = eventDocument,
                Metadata = EventMetadata.FromBson(eventDocument.GetBsonDocument("Metadata")),
                Handlers = EventHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }  
    }
}