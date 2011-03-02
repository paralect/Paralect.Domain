using System;

namespace Core.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public partial interface IEvent
    {
        IEventMetadata Metadata { get; set; }
    }
}