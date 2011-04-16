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
        /// Aggregate version
        /// </summary>
        private int _version;

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
            protected set { _id = value; }
        }

        /// <summary>
        /// Aggregate version
        /// </summary>
        public int Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
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

            var transitionEvents = _changes.Select(e => new TransitionEvent(
                dataTypeRegistry.GetTypeId(e.GetType()), e, null)).ToList();

            return new Transition(new TransitionId(_id, _version + 1), transitionEvents, null);
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
