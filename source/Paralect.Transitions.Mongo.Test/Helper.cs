using System;

namespace Paralect.Transitions.Mongo.Test
{
    public class Helper
    {
        public static MongoTransitionRepository GetRepository()
        {
            return new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(), 
                GetConnectionString());
        }

        public static String GetConnectionString()
        {
            return "mongodb://admin(admin):1@localhost:27020/test";
        }
    }
}
