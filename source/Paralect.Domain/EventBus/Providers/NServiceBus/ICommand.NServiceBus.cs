using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace Paralect.Domain
{
    public partial interface ICommand : IMessage
    {
    }
}
