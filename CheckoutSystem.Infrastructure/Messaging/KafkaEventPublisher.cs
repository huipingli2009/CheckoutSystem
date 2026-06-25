using CheckoutSystem.Application.Common;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CheckoutSystem.Infrastructure.Messaging
{
    public class KafkaEventPublisher : IEventPublisher, IDisposable
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher(IConfiguration configuration)
        {
            var boostrapServers = configuration["Kafka:BootstrapServers"]
                ?? "Localhost:9092"; // Default to localhost if not configured

            var config = new ProducerConfig
            {
                BootstrapServers = boostrapServers,
                Acks = Acks.All, // Ensure all replicas acknowledge the message
                EnableIdempotence = true, // Enable idempotent producer to avoid duplicates
                MessageSendMaxRetries = 3 // Retry sending messages up to 3 times
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
       
        public async Task PublishAsync<T>(string topic, string key, T message, CancellationToken cancellationToken = default) where T : class
        {
            var payload = JsonSerializer.Serialize(message);
            var kafkaMssage = new Message<string, string>
            {
                Key = key,
                Value = payload
            }; 
            
            try
            {
                // Deliver message to the local Kafka broker partition
                await _producer.ProduceAsync(topic, kafkaMssage, cancellationToken);
            }
            catch (ProduceException<string, string> ex)
            {
                throw new InvalidOperationException($"Failed to dispatch event to Kafka topic '{topic}': {ex.Error.Reason}", ex);
            }   
        }

        public void Dispose()
        {
            // Flush remaining buffered messages safely before terminating connection pools
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
            GC.SuppressFinalize(this);  
        }
    }
}
