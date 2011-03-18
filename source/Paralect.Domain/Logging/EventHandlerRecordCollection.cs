using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class EventHandlerRecordCollection
    {
        /// <summary>
        /// List of events records
        /// </summary>
        private List<EventHandlerRecord> _records = new List<EventHandlerRecord>();

        /// <summary>
        /// List of records
        /// </summary>
        public List<EventHandlerRecord> Items
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
        public EventHandlerRecordCollection(List<EventHandlerRecord> records)
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
        public static EventHandlerRecordCollection FromBson(BsonValue doc)
        {
            var list = new List<EventHandlerRecord>();

            if (!doc.IsBsonArray)
                return new EventHandlerRecordCollection(list);

            var evnts = doc.AsBsonArray;
            foreach (var evnt in evnts)
            {
                list.Add(EventHandlerRecord.FromBson(evnt.AsBsonDocument));
            }

            return new EventHandlerRecordCollection(list);
        }
    }
}
