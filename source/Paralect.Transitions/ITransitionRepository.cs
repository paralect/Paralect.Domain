using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    public interface ITransitionRepository
    {
        void SaveTransition(Transition transition);
        List<Transition> GetTransitions(String streamId, Int32 fromVersion, Int32 toVersion);
        void RemoveTransition(String streamId, Int32 version);
        void RemoveStream(String streamId);
    }
}
