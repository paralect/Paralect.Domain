using System;
using MongoDB.Bson;

namespace Paralect.Domain.EventStore.Providers.MongoDB
{
    public static class MongoExtensions
    {
        public static DateTime DefaultDateTime = DateTime.Parse("1980/1/1");

        public static String GetString(this BsonDocument doc, String key, String defaultValue = "")
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsString)
                return defaultValue;

            return value.AsString;
        }

        public static DateTime GetDateTime(this BsonDocument doc, String key, DateTime defaultValue = default(DateTime))
        {
            var defaultDateTime = defaultValue == default(DateTime) ? DefaultDateTime : defaultValue;

            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsDateTime)
                return defaultDateTime;

            return value.AsDateTime;
        }


        public static Int32 GetInt32(this BsonDocument doc, String key, Int32 defaultValue = 0)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsInt32)
                return defaultValue;

            return value.AsInt32;
        }

        public static Double GetDouble(this BsonDocument doc, String key, Double defaultValue = 0)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsDouble)
                return defaultValue;

            return value.AsDouble;
        }

        public static BsonArray GetBsonArray(this BsonDocument doc, String key)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsBsonArray)
                return new BsonArray();

            return value.AsBsonArray;
        }

        public static BsonDocument GetBsonDocument(this BsonDocument doc, String key)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsBsonDocument)
                return new BsonDocument();

            return value.AsBsonDocument;
        }


    }
}
