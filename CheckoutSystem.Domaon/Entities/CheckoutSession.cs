using CheckoutSystem.Domaon.Events;
using CheckoutSystem.Domaon.Primitives;
using CheckoutSystem.Domaon.ValueObjects;
using System.Numerics;

namespace CheckoutSystem.Domaon.Entities
{
    public enum CheckoutStatus
    {
        Open,
        Completed,
        Cancelled
    }
    public class CheckoutSession : AggregateRoot
    {
        private readonly List<CheckoutItem> _items = [];

        public Guid CustomerId { get; private set; }
        public CheckoutStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IReadOnlyCollection<CheckoutItem> Items => _items.AsReadOnly();

        public CheckoutSession(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
            Status = CheckoutStatus.Open;
            CreatedAt = DateTime.UtcNow;
        }

        // Invariant Rule: Total must always automatically update based on item changes
        public Money GetTotal(string currency = "USD")
        {
            var totalAmount = _items
                            .Where(i => i.Price.Currency == currency)
                            .Sum(i => i.Quantity * i.Price.Amount);

            return Money.Create(totalAmount, currency);
        }

        // Guarded Business Method
        public void AddOtUpdateItem(Guid productId, string productName, Money price, int quantity)
        {
            if (Status != CheckoutStatus.Open)
                throw new InvalidOperationException("Cannot modify items in a non-open checkout session.");
            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                var newItem = new CheckoutItem(Guid.NewGuid(), productId, productName, price, quantity);
                _items.Add(newItem);
            }
        }

        // Guarded Business Method
        public void CompleteCheckout()
        {
            if (Status != CheckoutStatus.Open)
                throw new InvalidOperationException("Only open checkout sessions can be completed.");

            if (!_items.Any())
                throw new InvalidOperationException("Cannot complete checkout with no items.");
            
            Status = CheckoutStatus.Completed;

            // Calculate the total amount for the event contract
            var total = GetTotal();

            // Raise the domain event
            RaiseDomainEvent(new CheckoutCompletedDomainEvent(
                EventId: Guid.NewGuid(),
                CheckoutSessionId: Id,
                CustomerId: CustomerId,
                TotalAmount: total.Amount,
                Currency: total.Currency,
                OccurredOn: DateTime.UtcNow
            ));
        }

        // EF Core materialization constructor
        private CheckoutSession() : base() { }
    }
}
