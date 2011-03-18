using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acropolis.Foundation.Infrastructure.MongoServers;
using MongoDB.Bson;

namespace Acropolis.Foundation.EventSourcing.Logging
{
    public class CommandHandlerRecord
    {
        /// <summary>
        /// Handler unique id
        /// </summary>
        public String HandlerId { get; set; }

        /// <summary>
        /// Command unique id
        /// </summary>
        public String CommandId { get; set; }

        /// <summary>
        /// Date of start of handling
        /// </summary>
        public DateTime StartedDate { get; set; }

        /// <summary>
        /// Date of end of handling
        /// </summary>
        public DateTime EndedDate { get; set; }

        /// <summary>
        /// CLR full type name of command handler
        /// </summary>
        public String TypeName { get; set; }

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
        public static BsonDocument ToBson(CommandHandlerRecord record)
        {
            return new BsonDocument()
            {
                { "HandlerId", record.HandlerId ?? "" },
                { "CommandId", record.CommandId ?? "" },
                { "StartedDate", record.StartedDate },
                { "EndedDate", record.EndedDate },
                { "TypeName", record.TypeName ?? "" },
                { "ErrorMessage", record.ErrorMessage ?? "" },
                { "ErrorStackTrace", record.ErrorStackTrace ?? ""},
            };
        }

        public static CommandHandlerRecord FromBson(BsonDocument doc)
        {
            var handler = new CommandHandlerRecord()
            {
                HandlerId = doc.GetString("HandlerId"),
                CommandId = doc.GetString("CommandId"),
                StartedDate = doc.GetDateTime("StartedDate"),
                EndedDate = doc.GetDateTime("EndedDate"),
                ErrorMessage = doc.GetString("ErrorMessage"),
                ErrorStackTrace = doc.GetString("ErrorStackTrace"),
                TypeName = doc.GetString("TypeName"),
            };

            return handler;
        }
    }
}
