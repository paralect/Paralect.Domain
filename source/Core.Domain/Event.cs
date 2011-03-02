using System;

namespace Core.Domain
{
    /// <summary>
    /// Domain event
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// Metadata of event
        /// </summary>
        private IEventMetadata _metadata = new EventMetadata();

        /// <summary>
        /// Metadata of event
        /// </summary>
        public IEventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }
    }
}
