using System;
using NServiceBus;

namespace Core.Domain
{
    public partial interface IEvent : IMessage
    {
    }
}
