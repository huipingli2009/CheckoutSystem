using CheckoutSystem.Domaon.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Domaon.Events
{
    public record CheckoutCompletedDomainEvent(
        Guid EventId,
        Guid CheckoutSessionId,
        Guid CustomerId,
        decimal TotalAmount,
        string Currency,
        DateTime OccurredOn) : IDomainEvent;  
}
