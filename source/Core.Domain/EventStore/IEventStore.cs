using System;

namespace Core.Domain.EventStore
{
    public interface IEventStore
    {
        void SaveChangeset(Changeset changeset);
        ChangesetStream GetChangesetStream(String aggregateId);
    }
}
