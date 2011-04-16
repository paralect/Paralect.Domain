using System;

namespace Paralect.Transitions
{
    public class TransitionId
    {
        /// <summary>
        /// Unique stream id (usually means aggregate id)
        /// </summary>
        public String StreamId { get; private set; }

        /// <summary>
        /// Version of commit (sequence number) 
        /// </summary>
        public Int32 Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TransitionId(string streamId, int version)
        {
            if (String.IsNullOrEmpty(streamId))
                throw new ArgumentException("StreamId cannot be null or empty.");

            if (version <= 0)
                throw new ArgumentException("Transition version cannot be zero or negative.");

            StreamId = streamId;
            Version = version;
        }
    }
}
