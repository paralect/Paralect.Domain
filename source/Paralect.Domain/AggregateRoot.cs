using System;
using System.Collections.Generic;
using Paralect.Domain.Utilities;
using Paralect.Transitions;
using System.Linq;

namespace Paralect.Domain
{
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        protected string _id;

        /// <summary>
        /// Aggregate version. Version 0 means that object was just created.
        /// Once object will be saved it version will be >= 1.
        /// </summary>
        private int _version = 0;

        /// <summary>
        /// List of changes (i.e. list os pending events)
        /// </summary>
        private readonly List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        public String Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Aggregate version
        /// </summary>
        public int Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        protected AggregateRoot()
        {

        }

        /// <summary>
        /// Create changeset. Used to persist changes in aggregate
        /// </summary>
        /// <returns></returns>
        public Transition CreateTransition(IDataTypeRegistry dataTypeRegistry)
        {
            if (String.IsNullOrEmpty(_id))
                throw new Exception(String.Format("ID was not specified for domain object. AggregateRoot [{0}] doesn't have correct ID. Maybe you forgot to set an _id field?", this.GetType().FullName));

            var transitionEvents = new List<TransitionEvent>();
            foreach (var e in _changes)
            {
                e.Metadata.StoredDate = DateTime.UtcNow;
                e.Metadata.TypeName = e.GetType().Name;
                transitionEvents.Add(new TransitionEvent(dataTypeRegistry.GetTypeId(e.GetType()), e, null));
            }

            return new Transition(new TransitionId(_id, _version + 1), DateTime.UtcNow, transitionEvents, null);
        }

        /// <summary>
        /// Load aggreagate from history
        /// </summary>
        public void LoadFromTransitionStream(ITransitionStream stream)
        {
            foreach (var transition in stream.Read())
            {
                foreach (var evnt in transition.Events)
                {
                    Apply((IEvent) evnt.Data, false);
                }

                _version = transition.Id.Version;
            }
        }

        /// <summary>
        /// Load aggregate from events
        /// </summary>
        /// <param name="events"></param>
        /// <param name="version"></param>
        public void LoadFromEvents(IEnumerable<IEvent> events, Int32 version = 1)
        {
            foreach (var evnt in events)
            {
                Apply(evnt, false);
            }

            _version = version;            
        }

        /// <summary>
        /// Apply event on aggregate 
        /// </summary>
        public void Apply(IEvent evnt)
        {
            Apply(evnt, true);
        }

        private void Apply(IEvent evnt, bool isNew)
        {
            this.AsDynamic().On(evnt);
            
            if (isNew) 
                _changes.Add(evnt);
        }
    }
}
