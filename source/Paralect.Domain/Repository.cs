using System;
using System.Linq;
using Paralect.Domain.EventBus;
using Paralect.Domain.Utilities;
using Paralect.Transitions;

namespace Paralect.Domain
{
    public class Repository : IRepository
    {
        private readonly ITransitionStorage _transitionStorage;
        private readonly IEventBus _eventBus;
        private readonly IDataTypeRegistry _dataTypeRegistry;
        private readonly ISnapshotRepository _snapshotRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Repository(ITransitionStorage transitionStorage,
                          IEventBus eventBus,
                          IDataTypeRegistry dataTypeRegistry, ISnapshotRepository snapshotRepository = null)
        {
            _transitionStorage = transitionStorage;
            _eventBus = eventBus;
            _dataTypeRegistry = dataTypeRegistry;
            _snapshotRepository = snapshotRepository;
        }

        public void Save(AggregateRoot aggregate)
        {
            if (String.IsNullOrEmpty(aggregate.Id))
                throw new ArgumentException("Aggregate id was not specified.");

            var transition = aggregate.CreateTransition(_dataTypeRegistry);

            using (var stream = _transitionStorage.OpenStream(transition.Id.StreamId))
            {
                stream.Write(transition);
            }

            if (_eventBus != null)
                _eventBus.Publish(transition.Events.Select(e => (IEvent)e.Data).ToList<IEvent>());

            if (_snapshotRepository != null)
            {
                aggregate.Version += 1; // we have to increase aggregate version, because of new transition increase aggregate version
                _snapshotRepository.Save(new Snapshot(aggregate.Id, transition.Id.Version, aggregate));
            }
        }

        public TAggregate GetById<TAggregate>(String id)
            where TAggregate : AggregateRoot
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException("Aggregate id was not specified.");
            Snapshot snapshot = null;

            if (_snapshotRepository != null)
                snapshot = _snapshotRepository.Load<TAggregate>(id);
            var obj = snapshot == null
                ? AggregateCreator.CreateAggregateRoot<TAggregate>()
                : (TAggregate)snapshot.Payload;
            var fromVersion = snapshot == null ? 0 : snapshot.StreamVersion + 1;
            var stream = _transitionStorage.OpenStream(id, fromVersion, int.MaxValue);
            obj.LoadFromTransitionStream(stream);
            return obj;
        }
    }
}
