using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    /// <summary>
    /// 
    /// </summary>
    public class TransitionEvent
    {
        /// <summary>
        /// Type of event. By default this is a fully qualified name of CLR type.
        /// But can be anything that can help identify event type during deserialization phase.
        /// </summary>
        public String TypeId { get; private set; }

        /// <summary>
        /// Data or body of event
        /// </summary>
        public Object Data { get; private set; }

        /// <summary>
        /// Metadata of event
        /// </summary>
        public Dictionary<String, Object> Metadata { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TransitionEvent(String typeId, Object data, Dictionary<String, Object> metadata)
        {
            TypeId = typeId;
            Data = data;
            Metadata = metadata ?? new Dictionary<String, Object>();
        }
    }
}
