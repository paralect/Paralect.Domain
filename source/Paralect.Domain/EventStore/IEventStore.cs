using System;

namespace Paralect.Domain.EventStore
{
    public interface IEventStore
    {
        void SaveChangeset(Changeset changeset);
        ChangesetStream GetChangesetStream(String aggregateId);
    }
}
