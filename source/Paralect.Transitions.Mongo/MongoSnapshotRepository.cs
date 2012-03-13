using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;

namespace Paralect.Transitions.Mongo
{
    public class MongoSnapshotRepository : ISnapshotRepository
    {
        private readonly MongoTransitionServer _transitionServer;

        public MongoSnapshotRepository(string connectionString)
        {
            _transitionServer = new MongoTransitionServer(connectionString);
        }

        public void Save(Snapshot snapShot, int minTransitionsForSnapshot = 30)
        {
            //Create snapshot for each # events or if snapshot not exists and # of events > then specified min
            if (snapShot.StreamVersion % minTransitionsForSnapshot == 0 || 
                (snapShot.StreamVersion > minTransitionsForSnapshot && IsSnapshotNotExists(snapShot.StreamId)))
            {
                var result = new BsonDocument();
                result["_id"] = GetSnapShotId(snapShot.StreamId, snapShot.StreamVersion);
                result["snapshot"] = JsonConvert.SerializeObject(snapShot.Payload);

                _transitionServer.Snapshots.Save(result);
            }
        }

        private bool IsSnapshotNotExists(string streamId)
        {
            return _transitionServer.Snapshots.Find(Query.EQ("_id.StreamId", streamId))
                    .SetLimit(1)
                    .SetFields(Fields.Include("_id"))
                    .FirstOrDefault() == null;
        }

        public Snapshot Load<T>(string id)
        {
            var query = Query.EQ("_id.StreamId", id);
            var bsonDoc = _transitionServer.Snapshots
                .Find(query)
                .SetLimit(1)
                .SetSortOrder(SortBy.Descending("_id.Version"))
                .FirstOrDefault();

            if (bsonDoc == null)
                return null;

            var value = bsonDoc["snapshot"].AsString;
            var docId = bsonDoc["_id"].AsBsonDocument;

            return new Snapshot(docId["StreamId"].AsString, docId["Version"].AsInt32, JsonConvert.DeserializeObject<T>(value));
        }

        private BsonDocument GetSnapShotId(string id, int version)
        {
            return new BsonDocument()
            {
                new BsonElement("StreamId", id),
                new BsonElement("Version", version)
            };
        }
    }
}
