using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    public class TransitionStream : ITransitionStream
    {
        private readonly string _streamId;

        private readonly Int32 _version;
        
        private readonly ITransitionRepository _transitionRepository;

        private readonly List<Transition> _transitions;

        private readonly List<TransitionEvent> _uncommitedEvents;

        public TransitionStream(String streamId, ITransitionRepository transitionRepository, List<Transition> transitions)
        {
            _streamId = streamId;
            _transitionRepository = transitionRepository;
            _transitions = transitions ?? new List<Transition>();
            _version = _transitions.Count > 0 ? _transitions[_transitions.Count - 1].Id.Version : 0;

            _uncommitedEvents = new List<TransitionEvent>();
        }

        public void AddEvent(TransitionEvent evnt)
        {
            _uncommitedEvents.Add(evnt);
        }

        public void Commit()
        {
            var id = new TransitionId(_streamId, _version + 1);
            var transition = new Transition(id, _uncommitedEvents, null);
            _transitionRepository.SaveTransition(transition);
        }

        public void Rollback()
        {
            _uncommitedEvents.Clear();
        }
    }
}
