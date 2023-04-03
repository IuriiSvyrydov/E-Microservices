

using EventBusRabbitMQ.Events.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<EventBusRabbitMQProducer> _logger;
        private readonly int _retryCount;
        public EventBusRabbitMQProducer(IRabbitMQConnection connection,
            ILogger<EventBusRabbitMQProducer> logger, int retryCount = 5)
        {
             _connection = connection;
            _logger = logger;
            _retryCount = retryCount;
        }
        public void Publish(string queueName, IEvent @event)
        {
            //check connection
            if(!_connection.IsConnected) _connection.TryConnect();
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                                    .Or<SocketException>()
                                    .WaitAndRetry(_retryCount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), (ex, time) =>
                                    {
                                        _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})"
                                            , @event.RequestId, $"{time.TotalSeconds:n1}",ex.Message);
                                    });
            using (var chanel = _connection.CreateModel())
            {
                chanel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject( @event );
                var body = Encoding.UTF8.GetBytes( message );
                policy.Execute(() =>
                {
                    IBasicProperties properties = chanel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    chanel.ConfirmSelect();
                    chanel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                    chanel.WaitForConfirmsOrDie();
                    chanel.BasicAcks += (sender, eventsArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");

                        //TODO in=mplement handle
                    };
                   

                });
            }
        }

      
    }
}
