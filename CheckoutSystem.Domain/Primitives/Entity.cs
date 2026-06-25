namespace CheckoutSystem.Domain.Primitives
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity(Guid id)
        {
            if(id == Guid.Empty) { throw new ArgumentNullException("id"); }
        
            Id = id;
        }

        // EFCore requires a parameterless constructor for materialization
        protected Entity() { }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other) return false;
            if(ReferenceEquals(this, other)) return true;
            if(GetType() != other.GetType()) return false;

            return Id == other.Id;
        }
        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity? a, Entity? b) => a is not null && b is not null && a.Equals(b);
        public static bool operator !=(Entity? a, Entity? b) => !(a == b);
    }
}
