using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class EventRecordCollection
    {
        /// <summary>
        /// List of events records
        /// </summary>
        private List<EventRecord> _records = new List<EventRecord>();

        /// <summary>
        /// List of records
        /// </summary>
        public List<EventRecord> Items
        {
            get { return _records; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EventRecordCollection(List<EventRecord> records)
        {
            _records = records;
        }

        private Int32 _errors;

        public int Errors
        {
            get { return _errors; }
        }


        /// <summary>
        /// From Bson
        /// </summary>
        public static EventRecordCollection FromBson(BsonValue doc)
        {
            var list = new List<EventRecord>();

            if (!doc.IsBsonArray)
                return new EventRecordCollection(list);

            var evnts = doc.AsBsonArray;
            var errorsCount = 0;
            foreach (var evnt in evnts)
            {
                var record = EventRecord.FromBson(evnt.AsBsonDocument);
                list.Add(record);
                errorsCount += record.Handlers.Errors;
            }

            return new EventRecordCollection(list) { _errors = errorsCount };
        }
    }
}
