using CheckoutSystem.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Domain.Events
{
    public record CheckoutCompletedDomainEvent(
        Guid EventId,
        Guid CheckoutSessionId,
        Guid CustomerId,
        decimal TotalAmount,
        string Currency,
        DateTime OccurredOn) : IDomainEvent;  
}
