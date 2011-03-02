using System;
using System.Collections.Generic;

namespace Core.Domain
{
    /// <summary>
    /// Changeset - is a way to group a number of modifications (events) 
    /// for **one** Aggregate Root in one atomic package, 
    /// that may be canceled or persisted by Event Store.
    /// </summary>
    public class Changeset
    {
        /// <summary>
        /// Unique changeset ID
        /// </summary>
        public String ChangesetId { get; set; }

        /// <summary>
        /// Events list 
        /// </summary>
        public List<IEvent> Events { get; set; }

        /// <summary>
        /// Aggregate Id 
        /// </summary>
        public String AggregateId { get; set; }

        /// <summary>
        /// Version of event group 
        /// </summary>
        public Int32 Version { get; set; }

        /// <summary>
        /// Initialization of event group
        /// </summary>
        public Changeset(string aggregateId, int version, string changesetId, List<IEvent> events)
        {
            ChangesetId = changesetId;
            AggregateId = aggregateId;
            Events = events ?? new List<IEvent>();
            Version = version;
        }

        public Changeset(String aggregateId, Int32 version, String changesetId) : 
            this(aggregateId, version, changesetId, new List<IEvent>())
        {
        }

        public Changeset(String aggregateId, Int32 version) : 
            this(aggregateId, version, Guid.NewGuid().ToString(), new List<IEvent>())
        {
        }

        public Int32 Count
        {
            get { return Events.Count; }
        }

        /// <summary>
        /// Mark changes as commited
        /// </summary>
        public void Clear()
        {
            Events.Clear();
        }

        public void AddEvent(IEvent evnt)
        {
            Events.Add(evnt);
        }
    }
}