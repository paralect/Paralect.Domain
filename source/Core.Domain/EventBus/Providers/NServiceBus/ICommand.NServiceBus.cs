using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace Core.Domain
{
    public partial interface ICommand : IMessage
    {
    }
}
