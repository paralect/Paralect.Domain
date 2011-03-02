using System;
using MongoDB.Driver;

namespace Core.Domain.EventStore.Providers.MongoDB.Servers
{
    public class EventsServer
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
        /// Collection for storing events data
        /// </summary>
        private readonly string _collectionName;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public EventsServer(String connectionString, String collectionName)
        {
            _collectionName = collectionName;
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);
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
        /// Get events collection
        /// </summary>
        public MongoCollection Events
        {
            get { return Database.GetCollection(_collectionName); }
        }
    }
}
