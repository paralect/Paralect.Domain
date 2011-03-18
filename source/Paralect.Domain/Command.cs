using System;

namespace Paralect.Domain
{
    /// <summary>
    /// Domain Command
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Command metadata
        /// </summary>
        private ICommandMetadata _metadata = new CommandMetadata();

        /// <summary>
        /// Command metadata
        /// </summary>
        public ICommandMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }
    }
}
