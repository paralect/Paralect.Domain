using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acropolis.Foundation.Infrastructure.MongoServers;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    /// <summary>
    /// Represent one physical log record
    /// </summary>
    public class LogRecord
    {
        /// <summary>
        /// Command (always one)
        /// </summary>
        public CommandRecord Command { get; set; }

        /// <summary>
        /// Events rised for command
        /// </summary>
        public EventRecordCollection Events { get; set; }

        private Int32 _errors;

        public int Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// From BSON
        /// </summary>
        public static LogRecord FromBson(BsonDocument doc)
        {
            var record = new LogRecord();
            record.Command = CommandRecord.FromBson(doc);
            record.Events = EventRecordCollection.FromBson(doc.GetBsonArray("Events"));
            record._errors = record.Command.Handlers.Errors + record.Events.Errors;
            return record;
        }  

    }
}
