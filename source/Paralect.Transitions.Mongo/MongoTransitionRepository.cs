using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Paralect.Transitions.Mongo
{
    public class MongoTransitionRepository : ITransitionRepository
    {
        private readonly IDataTypeRegistry _dataTypeRegistry;
        private const string _concurrencyException = "E1100";
        private readonly MongoTransitionServer _server;
        private readonly MongoTransitionSerializer _serializer;

        public MongoTransitionRepository(IDataTypeRegistry dataTypeRegistry, String connectionString, String collectionName = "transitions")
        {
            _dataTypeRegistry = dataTypeRegistry;
            _serializer = new MongoTransitionSerializer(dataTypeRegistry);
            _server = new MongoTransitionServer(connectionString, collectionName);
            
            _server.Transitions.EnsureIndex(
                IndexKeys.Ascending("_id.StreamId", "_id.Version"));
        }

        public void SaveTransition(Transition transition)
        {
            // skip saving empty transition
            if (transition.Events.Count < 1)
                return;

            var doc = _serializer.Serialize(transition);

            try
            {
                _server.Transitions.Insert(doc, SafeMode.True);
            }
            catch (MongoException e)
            {
                if (!e.Message.Contains(_concurrencyException))
                    throw;

                throw new ConcurrencyException(String.Format("Transition ({0}, {1}) already exists.", transition.Id.StreamId, transition.Id.Version));
            }
        }

        public List<Transition> GetTransitions(string streamId, int fromVersion, int toVersion)
        {
            var query = Query.And(
                Query.EQ("_id.StreamId", streamId),
                Query.GTE("_id.Version", fromVersion),
                Query.LTE("_id.Version", toVersion));

            var sort = SortBy.Ascending("_id.Version");

            var docs = _server.Transitions.FindAs<BsonDocument>(query)
                .SetSortOrder(sort)
                .ToList();

            // Check that such stream exists
            if (docs.Count < 1)
                throw new ArgumentException(String.Format("There is no stream in store with id {0}", streamId));

            var transitions = docs.Select(_serializer.Deserialize).ToList();

            return transitions;
        }

        public void RemoveTransition(string streamId, int version)
        {
            var query = new BsonDocument {
                {"_id", new BsonDocument {
                    { "StreamId", streamId },
                    { "Version", version }
                }}
            };

            _server.Transitions.Remove(new QueryDocument(query));
        }

        public void RemoveStream(String streamId)
        {
            var query = new BsonDocument { {"_id.StreamId", streamId } };
            _server.Transitions.Remove(new QueryDocument(query));            
        }
    }
}
