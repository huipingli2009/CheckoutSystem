using CheckoutSystem.Domain.Primitives;
using CheckoutSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Domain.Entities
{
    public class CheckoutItem : Entity
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public Money Price { get; private set; }
        public int Quantity { get; private set; }

        internal CheckoutItem(Guid id, Guid productId, string productName, Money price, int quantity) : base(id)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            ProductId = productId;
            ProductName = productName;
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Quantity = quantity;
        }

        internal void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
        }

        // EF Core materialization constructor
        private CheckoutItem() : base() { }
    }
}
