using System.Collections.Generic;
using Paralect.ServiceBus;

namespace Paralect.Domain.EventBus
{
    public class ParalectServiceBusEventBus : IEventBus
    {
        private readonly IServiceBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ParalectServiceBusEventBus(IServiceBus bus)
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