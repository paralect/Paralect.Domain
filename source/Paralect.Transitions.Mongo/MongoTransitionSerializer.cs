using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Paralect.Transitions.Mongo
{
    public class MongoTransitionSerializer
    {
        private readonly IDataTypeRegistry _dataTypeRegistry;
        private readonly MongoTransitionDataSerializer _dataSerializer;

        public MongoTransitionSerializer(IDataTypeRegistry dataTypeRegistry)
        {
            _dataTypeRegistry = dataTypeRegistry;
            _dataSerializer = new MongoTransitionDataSerializer(dataTypeRegistry);
        }

        /// <summary>
        /// Serialize transiton to BsonDocument
        /// </summary>
        public BsonDocument Serialize(Transition transition)
        {
            return new BsonDocument 
            {
                { "_id", SerializeTransitionId(transition.Id) },
                { "Events", SerializeTransitionEvents(transition.Events) },
                { "Metadata", SerializeMetadata(transition.Metadata) },
            };
        }

        /// <summary>
        /// Deserialize from BsonDocument to Transition
        /// </summary>
        public Transition Deserialize(BsonDocument doc)
        {
            var transitionId = DeserializeTransitionId(doc["_id"]);
            var events = DeserializeTransitionEvents(doc["Events"]);
            var metadata = DeserializeMetadata(doc["Metadata"]);
            return new Transition(transitionId, events, metadata);
        }


        #region Transition Id Serialization

        private BsonDocument SerializeTransitionId(TransitionId transitionId)
        {
            return new BsonDocument 
            {
                { "StreamId", transitionId.StreamId }, 
                { "Version", transitionId.Version }
            };
        }

        private TransitionId DeserializeTransitionId(BsonValue id)
        {
            if (!id.IsBsonDocument)
                throw new Exception("Transition _id should be a BsonDocument.");

            var transitionId = id.AsBsonDocument;
            var streamId = transitionId["StreamId"].AsString;
            var version = transitionId["Version"].AsInt32;

            return new TransitionId(streamId, version);
        }

        #endregion


        #region Metadata Serialization

        private BsonValue SerializeMetadata(Dictionary<String, Object> metadata)
        {
            return BsonDocumentWrapper.Create(metadata);
        }

        private Dictionary<String, Object> DeserializeMetadata(BsonValue bsonValue)
        {
            if (!bsonValue.IsBsonDocument)
                throw new Exception("Cannot deserialize metadata (it is not a BsonDocument)");

            return BsonSerializer.Deserialize<Dictionary<String, Object>>(bsonValue.AsBsonDocument);
        }

        #endregion


        #region Transition Events Serialization

        private BsonArray SerializeTransitionEvents(List<TransitionEvent> events)
        {
            BsonArray array = new BsonArray();

            foreach (var e in events)
            {
                array.Add(new BsonDocument()
                {
                    { "TypeId", e.TypeId },
                    { "Metadata", SerializeMetadata(e.Metadata) },
                    { "Data", _dataSerializer.Serialize(e.Data) }
                });
            }

            return array;
        }

        private List<TransitionEvent> DeserializeTransitionEvents(BsonValue bsonValue)
        {
            if (!bsonValue.IsBsonArray)
                throw new Exception("Events should always be an array.");

            var eventArray = bsonValue.AsBsonArray;

            var events = new List<TransitionEvent>();
            foreach (var eventValue in eventArray)
            {
                var eventDoc = eventValue.AsBsonDocument;

                var eventMetadata = DeserializeMetadata(eventDoc["Metadata"]);
                var eventTypeId = eventDoc["TypeId"].AsString;
                var eventData = _dataSerializer.Deserialize(eventDoc["Data"].AsBsonDocument, _dataTypeRegistry.GetType(eventTypeId));

                events.Add(new TransitionEvent(eventTypeId, eventData, eventMetadata));
            }

            return events;
        }

        #endregion
    }
}
