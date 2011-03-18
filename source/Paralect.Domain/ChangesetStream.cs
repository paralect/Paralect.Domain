using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paralect.Domain
{
    /// <summary>
    /// Stream of changesets from which aggregate will be restored.
    /// Important: Changesets MUST BE in the correct ascending order sorted by changeset version!
    /// </summary>
    public class ChangesetStream
    {
        /// <summary>
        /// Version of aggregate root
        /// </summary>
        private Int32 _lastVersion;

        /// <summary>
        /// List of persisted changesets
        /// </summary>
        private List<Changeset> _changesets;

        /// <summary>
        /// Version of aggregate root
        /// </summary>
        public int LastVersion
        {
            get { return _lastVersion; }
        }

        /// <summary>
        /// List of persisted changesets
        /// </summary>
        public List<Changeset> Changesets
        {
            get { return _changesets; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ChangesetStream(List<Changeset> changesets, Int32 lastVersion)
        {
            _changesets = changesets;
            _lastVersion = lastVersion;
        }
    }
}
