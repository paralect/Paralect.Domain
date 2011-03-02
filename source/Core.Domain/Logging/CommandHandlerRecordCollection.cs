using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class CommandHandlerRecordCollection
    {
        /// <summary>
        /// List of events records
        /// </summary>
        private List<CommandHandlerRecord> _records = new List<CommandHandlerRecord>();

        /// <summary>
        /// List of records
        /// </summary>
        public List<CommandHandlerRecord> Items
        {
            get { return _records; }
        }

        private int _errors;

        public Int32 Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CommandHandlerRecordCollection(List<CommandHandlerRecord> records)
        {
            foreach (var record in records)
            {
                if (record.ErrorMessage != "")
                    _errors++;
            }

            _records = records;
        }

        /// <summary>
        /// From Bson
        /// </summary>
        public static CommandHandlerRecordCollection FromBson(BsonValue doc)
        {
            var list = new List<CommandHandlerRecord>();

            if (!doc.IsBsonArray)
                return new CommandHandlerRecordCollection(list);

            var evnts = doc.AsBsonArray;
            foreach (var evnt in evnts)
            {
                list.Add(CommandHandlerRecord.FromBson(evnt.AsBsonDocument));
            }

            return new CommandHandlerRecordCollection(list);
        }
    }
}