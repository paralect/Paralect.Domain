using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Paralect.Transitions.Mongo
{
    public class MongoTransitionDataSerializer
    {
        private readonly IDataTypeRegistry _dataTypeRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MongoTransitionDataSerializer(IDataTypeRegistry dataTypeRegistry)
        {
            _dataTypeRegistry = dataTypeRegistry;
        }

        public Object Deserialize(BsonDocument doc, Type type)
        {
            return BsonSerializer.Deserialize(doc, type);
        }

        public BsonDocument Serialize(Object obj)
        {
            BsonDocument data = new BsonDocument();

            var writer = BsonWriter.Create(data);
            BsonSerializer.Serialize(writer, obj.GetType(), obj);

            return data;
        }
    }
}
