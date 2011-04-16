using System.Collections.Generic;
using Paralect.ServiceBus;

namespace Paralect.Domain.EventBus.Providers.ParalectServiceBus
{
    public class ParalectServiceBusEventBus : IEventBus
    {
        private readonly IBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ParalectServiceBusEventBus(IBus bus)
        {
            _bus = bus;
        }

        public void Publish(IEvent eventMessage)
        {
            _bus.Publish(eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            foreach (var eventMessage in eventMessages)
            {
                _bus.Publish(eventMessage);
            }
        }
    }
}