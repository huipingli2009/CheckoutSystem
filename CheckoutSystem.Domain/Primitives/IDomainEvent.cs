using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Domain.Primitives
{
    // Implementing MediatR's INotification allows these events to be dispatched locally via MediatR pipelines
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }    
    }
}
