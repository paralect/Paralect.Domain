using System.Collections.Generic;

namespace Core.Domain.EventBus.Providers.StubBus
{
    public class StubEventBus : IEventBus
    {
        public void Publish(IEvent eventMessage)
        {

        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {

        }
    }
}