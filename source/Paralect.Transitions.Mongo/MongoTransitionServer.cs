using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Paralect.Transitions.Mongo
{
    public class MongoTransitionServer
    {
        private readonly MongoServer _server;

        private readonly string _databaseName;

        /// <summary>
        /// Collection for storing commits data
        /// </summary>
        private const string TransaitonsCollectionName = "transitions";
        private const string SnapshotsCollectionName = "snapshots";

        private readonly MongoCollectionSettings<BsonDocument> _transitionSettings;
        private readonly MongoCollectionSettings<BsonDocument> _snapshotSettings;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoTransitionServer(String connectionString)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);

            _transitionSettings = Database.CreateCollectionSettings<BsonDocument>(TransaitonsCollectionName);
            _transitionSettings.SafeMode = SafeMode.True;
            _transitionSettings.AssignIdOnInsert = false;

            _snapshotSettings = Database.CreateCollectionSettings<BsonDocument>(SnapshotsCollectionName);
            _snapshotSettings.SafeMode = SafeMode.True;
            _snapshotSettings.AssignIdOnInsert = false;
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoDB.Driver.MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        /// <summary>
        /// Get commits collection
        /// </summary>
        public MongoCollection<BsonDocument> Transitions
        {
            get { return Database.GetCollection(_transitionSettings); }
        }

        public MongoCollection<BsonDocument> Snapshots
        {
            get { return Database.GetCollection(_snapshotSettings); }
        }
    }
}