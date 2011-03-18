using System;
using Acropolis.Foundation.Infrastructure.MongoServers;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class EventHandlerRecord
    {
        /// <summary>
        /// Handler unique id
        /// </summary>
        public String HandlerId { get; set; }

        /// <summary>
        /// Command Id
        /// </summary>
        public String CommandId { get; set; }

        /// <summary>
        /// Event Id
        /// </summary>
        public String EventId { get; set; }

        /// <summary>
        /// CLR full type name of handler
        /// </summary>
        public String TypeName { get; set; }

        /// <summary>
        /// Date of start of handling
        /// </summary>
        public DateTime StartedDate { get; set; }

        /// <summary>
        /// Date of end of handling
        /// </summary>
        public DateTime EndedDate { get; set; }

        /// <summary>
        /// Error Message (if exists, "" otherwise)
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// Error stack trace (if exists, "" otherwise)
        /// </summary>
        public String ErrorStackTrace { get; set; }



        /// <summary>
        /// To Bson
        /// </summary>
        public static BsonDocument ToBson(EventHandlerRecord record)
        {
            return new BsonDocument
            {
                { "HandlerId", record.HandlerId ?? "" },
                { "EventId", record.EventId ?? "" },
                { "CommandId", record.CommandId ?? "" },
                { "TypeName", record.TypeName ?? "" },
                { "StartedDate", record.StartedDate },
                { "EndedDate", record.EndedDate },
                { "ErrorMessage", record.ErrorMessage ?? "" },
                { "ErrorStackTrace", record.ErrorStackTrace ?? "" },
            };
        }


        public static EventHandlerRecord FromBson(BsonDocument doc)
        {
            var handler = new EventHandlerRecord
            {
                HandlerId = doc.GetString("HandlerId"),
                EventId = doc.GetString("EventId"),
                CommandId = doc.GetString("CommandId"),
                TypeName = doc.GetString("TypeName"),
                StartedDate = doc.GetDateTime("StartedDate"),
                EndedDate = doc.GetDateTime("EndedDate"),
                ErrorMessage = doc.GetString("ErrorMessage"),
                ErrorStackTrace = doc.GetString("ErrorStackTrace"),
            };

            return handler;
        }

    }
}