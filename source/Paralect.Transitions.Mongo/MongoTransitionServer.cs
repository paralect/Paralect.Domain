using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Paralect.Transitions.Mongo
{
    public class MongoTransitionServer
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// Collection for storing commits data
        /// </summary>
        private readonly string _collectionName;

        private readonly MongoCollectionSettings<BsonDocument> _transitionSettings;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoTransitionServer(String connectionString, String collectionName)
        {
            _collectionName = collectionName;
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);

            _transitionSettings = Database.CreateCollectionSettings<BsonDocument>(_collectionName);
            _transitionSettings.SafeMode = SafeMode.True;
            _transitionSettings.AssignIdOnInsert = false;
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
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
    }
}