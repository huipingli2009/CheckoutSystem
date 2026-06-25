using CheckoutSystem.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        private Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new ArgumentException("Currency must be a valid 3-letter ISO code.", nameof(currency));
            Amount = amount;
            Currency = currency;        
        }

        public static Money Create(decimal amount, string currency)
        {
            return new Money(amount, currency);
        }

        public static Money Zero(string currency) => new Money(0, currency);
        public Money Add(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (Currency != other.Currency) throw new InvalidOperationException("Cannot add money with different currencies.");
            return new Money(Amount + other.Amount, Currency);
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;    
            yield return Currency;
        }        
    }

    // Simple internal helper for string sharing optimization if needed, or just use .ToUpper()
    internal static class StringExtensions
    {
        public static string ToUpperShared(this string str) => str.ToUpperInvariant();
    }
}
