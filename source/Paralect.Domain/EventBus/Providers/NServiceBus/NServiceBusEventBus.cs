using System.Collections.Generic;
using NServiceBus;

namespace Paralect.Domain.EventBus.Providers.NServiceBus
{
    public class NServiceBusEventBus : IEventBus
    {
        private readonly IBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public NServiceBusEventBus(IBus bus)
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
