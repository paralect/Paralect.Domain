using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    public interface ITransitionStream : IDisposable
    {
        IEnumerable<Transition> Read();
        void Write(Transition transition);
    }
}