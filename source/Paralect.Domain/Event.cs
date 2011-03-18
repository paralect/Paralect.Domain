using System;

namespace Paralect.Domain
{
    /// <summary>
    /// Domain event
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// Metadata of event
        /// </summary>
        private EventMetadata _metadata = new EventMetadata();

        /// <summary>
        /// Metadata of event
        /// </summary>
        public EventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }
    }
}
