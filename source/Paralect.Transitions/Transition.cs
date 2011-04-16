using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    /// <summary>
    /// Transition - is a way to group a number of modifications (events) 
    /// for **one** Stream (usually Aggregate Root) in one atomic package, 
    /// that can be either canceled or persisted by Event Store.
    /// </summary>    
    public class Transition
    {
        public TransitionId Id { get; private set; }

        /// <summary>
        /// Events in commit
        /// </summary>
        public List<TransitionEvent> Events { get; private set; }

        /// <summary>
        /// Metadata of commit
        /// </summary>
        public Dictionary<String, Object> Metadata { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Transition(TransitionId transitionId, List<TransitionEvent> events, Dictionary<string, object> metadata)
        {
            Id = transitionId;
            Events = events;
            Metadata = metadata ?? new Dictionary<String, Object>();
        }
    }
}
