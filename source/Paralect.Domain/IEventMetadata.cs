using System;
using MongoDB.Bson.DefaultSerializer;

namespace Paralect.Domain
{
    public interface IEventMetadata
    {
        /// <summary>
        /// Unique Id of event
        /// </summary>
        String EventId { get; set; }

        /// <summary>
        /// Command Id of command that initiate this event
        /// </summary>
        String CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiated this event
        /// </summary>
        String UserId { get; set; }

        /// <summary>
        /// Datetime when event was stored in Event Store.
        /// </summary>
        DateTime StoredDate { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name
        /// </summary>
        String TypeName { get; set; }
    }
}