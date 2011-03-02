using System.Collections.Generic;

namespace Core.Domain.EventBus.Providers.NServiceBus
{
    public class NServiceBusEventBus : IEventBus
    {
        public void Publish(IEvent eventMessage)
        {
            
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            
        }
    }
}
