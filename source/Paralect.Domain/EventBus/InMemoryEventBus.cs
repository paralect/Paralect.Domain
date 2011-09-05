using System.Collections.Generic;

namespace Paralect.Domain.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        public List<IEvent> Events = new List<IEvent>();

        public void Publish(IEvent eventMessage)
        {
            Events.Add(eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            Events.AddRange(eventMessages);
        }
    }
}