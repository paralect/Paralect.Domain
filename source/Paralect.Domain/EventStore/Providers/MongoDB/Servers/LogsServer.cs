using System;
using MongoDB.Driver;

namespace Paralect.Domain.EventStore.Providers.MongoDB.Servers
{
    /// <summary>
    /// Wrapper for Logs database
    /// </summary>
    public class LogsServer
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
        /// Collection for storing logs data
        /// </summary>
        private readonly string _collectionName;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public LogsServer(String connectionString, String collectionName)
        {
            _collectionName = collectionName;
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase GetDatabase()
        {
            return _server.GetDatabase(_databaseName);
        }

        /// <summary>
        /// Get Log collection
        /// </summary>
        public MongoCollection GetLogsCollection()
        {
            return GetDatabase().GetCollection(_collectionName);
        }
    }
}
