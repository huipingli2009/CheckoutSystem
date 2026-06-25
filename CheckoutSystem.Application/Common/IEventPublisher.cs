using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Application.Common
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string topic, string key, T message, CancellationToken cancellationToken = default)
            where T : class;
    }
}
