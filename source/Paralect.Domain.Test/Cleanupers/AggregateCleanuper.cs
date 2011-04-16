using System;
using MongoDB.Driver.Builders;

namespace Paralect.Domain.Test.Cleanupers
{
    public class AggregateCleanuper : IDisposable
    {
        private readonly string _aggregateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AggregateCleanuper(String aggregateId)
        {
            _aggregateId = aggregateId;
        }

        public void Dispose()
        {
            if (String.IsNullOrEmpty(_aggregateId))
                return;

            Helper.GetTransitionRepository().RemoveStream(_aggregateId);
        }
    }
}
