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
            //return "mongodb://admin(admin):1@orsich-pc:27020/test";
            return "mongodb://localhost:27018/test";
        }
    }
}
