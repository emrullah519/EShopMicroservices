﻿

namespace Ordering.Domain.Abstractions
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> Events => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IDomainEvent[] ClearDomainEvents()
        {
            var dequeedEvents = _domainEvents.ToArray();
            _domainEvents.Clear();
            return dequeedEvents;
        }
    }
}
