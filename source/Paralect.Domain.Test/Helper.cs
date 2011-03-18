using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Domain.EventStore;
using Paralect.Domain.EventStore.Providers.MongoDB;
using Paralect.Domain.EventStore.Providers.MongoDB.Servers;
using Paralect.Domain.Test.Aggregates;

namespace Paralect.Domain.Test
{
    public class Helper
    {
        public static Repository GetRepository()
        {
            return new Repository(GetEventStore(), null);
        }

        public static IEventStore GetEventStore()
        {
            EventsServer server = GetEventServer();
            var store = new MongoDbEventStore(server);

            return store;
        }

        public static EventsServer GetEventServer()
        {
            return new EventsServer("mongodb://localhost:27018/test", "events");
        }
    }
}
