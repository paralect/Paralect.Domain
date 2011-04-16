using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Paralect.Domain
{
    [BsonDiscriminator("CommandMetadata")]
    public class CommandMetadata : ICommandMetadata
    {
        /// <summary>
        /// Unique Id of Command
        /// </summary>
        public String CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiate this command
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name of Command Type
        /// </summary>
        public String TypeName { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}