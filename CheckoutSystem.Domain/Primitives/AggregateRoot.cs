namespace CheckoutSystem.Domain.Primitives
{
    public class AggregateRoot : Entity
    {
        private readonly List<object> _domainEvents = [];
        protected AggregateRoot(Guid id) : base(id) { }
        protected AggregateRoot() { }

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
