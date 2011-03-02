using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain.EventStore.Providers.MongoDB;
using Core.Domain.EventStore;
using Core.Domain.EventStore.Providers.MongoDB.Servers;

using Core.Domain.Test.Aggregates;

namespace Core.Domain.Test
{
    public class Helper
    {
        public static Repository<User> GetUserRepository()
        {
            return new Repository<User>(GetEventStore(), null);
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
