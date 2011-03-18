using System;
using MongoDB.Driver.Builders;
using Paralect.Domain.EventStore.Providers.MongoDB.Servers;

namespace Paralect.Domain.Test.Cleanupers
{
    public class AggregateCleanuper : IDisposable
    {
        private readonly EventsServer _server;
        private readonly string _aggregateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AggregateCleanuper(EventsServer server, String aggregateId)
        {
            _server = server;
            _aggregateId = aggregateId;
        }

        public void Dispose()
        {
            if (String.IsNullOrEmpty(_aggregateId))
                return;

            _server.Events.Remove(Query.EQ("AggregateId", _aggregateId));
        }
    }
}
