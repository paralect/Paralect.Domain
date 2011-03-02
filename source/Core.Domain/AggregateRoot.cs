using System;
using System.Collections.Generic;
using Core.Domain.Utilities;

namespace Core.Domain
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
        public Changeset CreateChangeset()
        {
            return new Changeset(_id, _version + 1, Guid.NewGuid().ToString(), _changes);
        }

        /// <summary>
        /// Load aggreagate from history
        /// </summary>
        public void LoadsFromChangesetStream(ChangesetStream stream)
        {
            foreach (var changeset in stream.Changesets)
            {
                foreach (var evnt in changeset.Events)
                {
                    Apply(evnt, false);
                }
            }

            _version = stream.LastVersion;
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
