using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    public interface ITransitionRepository
    {
        void SaveTransition(Transition transition);
        List<Transition> GetTransitions(String streamId, Int32 fromVersion, Int32 toVersion);

        /// <summary>
        /// Get all transitions ordered ascendantly by Timestamp of transiton
        /// Should be used only for testing and for very simple event replying 
        /// </summary>
        List<Transition> GetTransitions();

        void RemoveTransition(String streamId, Int32 version);
        void RemoveStream(String streamId);
    }
}
