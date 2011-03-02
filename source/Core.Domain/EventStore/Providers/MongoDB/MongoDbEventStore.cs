using System;
using System.Linq;
using Core.Domain.EventStore.Providers.MongoDB.Serialization;
using Core.Domain.EventStore.Providers.MongoDB.Servers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Core.Domain.EventStore.Providers.MongoDB
{
    /// <summary>
    /// Make sure you have such index on event collection in Mongo:
    /// db.events.ensureIndex({AggregateId: 1, Version: 1}, {unique: true});
    /// </summary>
    public class MongoDbEventStore : IEventStore
    {
        /// <summary>
        /// Storage for events
        /// </summary>
        private readonly EventsServer _eventsServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MongoDbEventStore(EventsServer eventsServer)
        {
            _eventsServer = eventsServer;
        }

        /// <summary>
        /// Storing events in natural order
        /// </summary>
        public void SaveChangeset(Changeset changeset)
        {
            // skip saving empty set of events
            if (changeset.Events.Count < 1)
                return;

            var doc = ChangesetSerializer.Serialize(changeset);

            try
            {
                using(_eventsServer.Database.RequestStart())
                {
                    _eventsServer.Events.Insert(doc, SafeMode.False);
                    var error = _eventsServer.Server.GetLastError();

                    if (error.HasLastErrorMessage)
                        throw new Exception("Aggregate of such version already exists!");       
                }
                //Bus.Publish(events.ToArray());
            }
            catch(Exception e)
            {
                throw;
                //throw new ConcurrencyException();                
            }
        }

        public ChangesetStream GetChangesetStream(String aggregateId)
        {
            var docs = _eventsServer.Events.FindAs<BsonDocument>(Query.EQ("AggregateId", aggregateId))
                .SetSortOrder(SortBy.Ascending("Version"))
                .ToList();

            // Check that such aggregate exists
            if (docs.Count < 1)
                throw new ArgumentException(String.Format("There is no aggregate in store with id {0}", aggregateId));

            var changesets = docs.Select(ChangesetSerializer.Deserialize).ToList();

            var version = changesets.Last().Version;

            return new ChangesetStream(changesets, version);
        }
    }
}
