using System;

namespace Paralect.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public partial interface IEvent
    {
        EventMetadata Metadata { get; set; }
    }
}