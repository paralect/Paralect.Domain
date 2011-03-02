using System;

namespace Core.Domain
{
    /// <summary>
    /// Metadata of particular event
    /// </summary>
    public class EventMetadata : IEventMetadata
    {
        /// <summary>
        /// Unique Id of event
        /// </summary>
        public String EventId { get; set; }

        /// <summary>
        /// Command Id of command that initiate this event
        /// </summary>
        public String CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiated this event
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// Datetime when event was stored in Event Store.
        /// </summary>
        public DateTime StoredDate { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name
        /// </summary>
        public String TypeName { get; set; }
    }


}