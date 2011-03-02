using System;

namespace Core.Domain
{
    public interface IRepository<TAggregate> where TAggregate : AggregateRoot
    {
        void Save(AggregateRoot aggregate);
        TAggregate GetById(String id);
    }
}