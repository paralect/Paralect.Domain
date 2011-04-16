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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Repository(ITransitionStorage transitionStorage, IEventBus eventBus, IDataTypeRegistry _dataTypeRegistry)
        {
            _transitionStorage = transitionStorage;
            _eventBus = eventBus;
            this._dataTypeRegistry = _dataTypeRegistry;
        }

        public void Save(AggregateRoot aggregate)
        {
            var transition = aggregate.CreateTransition(_dataTypeRegistry);

            using (var stream = _transitionStorage.OpenStream(transition.Id.StreamId))
            {
                stream.Write(transition);
            }

/*            if (_eventBus != null)
                _eventBus.Publish(changeset.Events.Select(e=> e.Data).ToList<IEvent>());*/
        }

        public TAggregate GetById<TAggregate>(String id)
            where TAggregate : AggregateRoot
        {
            var stream = _transitionStorage.OpenStream(id);

            var obj = AggregateCreator.CreateAggregateRoot<TAggregate>();
            obj.LoadFromTransitionStream(stream);
            return obj;
        }
    }
}
