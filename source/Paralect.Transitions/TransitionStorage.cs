using System;
using System.Collections.Generic;

namespace Paralect.Transitions
{
    public class TransitionStorage : ITransitionStorage
    {
        private readonly ITransitionRepository _transitionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TransitionStorage(ITransitionRepository transitionRepository)
        {
            _transitionRepository = transitionRepository;
        }

        public ITransitionStream OpenStream(String streamId, int fromVersion, int toVersion)
        {
            var transitions = _transitionRepository.GetTransitions(streamId, fromVersion, toVersion);

            var stream = new TransitionStream(streamId, _transitionRepository, transitions);

            return stream;
        }

        public ITransitionStream CreateStream(String streamId)
        {
            return new TransitionStream(streamId, _transitionRepository, new List<Transition>());
        }
    }
}
