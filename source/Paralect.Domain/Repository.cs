using System;
using Paralect.Domain.EventBus;
using Paralect.Domain.EventStore;
using Paralect.Domain.Utilities;

namespace Paralect.Domain
{
    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Repository(IEventStore eventStore, IEventBus eventBus)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
        }

        public void Save(AggregateRoot aggregate)
        {
            var changeset = aggregate.CreateChangeset();

            _eventStore.SaveChangeset(changeset);

            if (_eventBus != null)
                _eventBus.Publish(changeset.Events);
        }

        public TAggregate GetById<TAggregate>(String id)
            where TAggregate : AggregateRoot
        {
            var stream = _eventStore.GetChangesetStream(id);

            var obj = AggregateCreator.CreateAggregateRoot<TAggregate>();
            obj.LoadsFromChangesetStream(stream);
            return obj;
        }
    }
}
