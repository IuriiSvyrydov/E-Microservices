using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace EventBusRabbitMQ
{
    public class DefaultRabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private  IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultRabbitMQConnection> _logger;
        private bool _disposed;
        public DefaultRabbitMQConnection(IConnectionFactory connectionFactory,
                                        
                                            int retryCount,
                                            ILogger<DefaultRabbitMQConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }
        public bool IsConnected
        {
            get
            {
                return _connection !=null&& _connection.IsOpen&& !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if(!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to prerfome this action");
            }
            return _connection.CreateModel();
        
        }

        public void Dispose()
        {
            if(_disposed) return;
            _disposed = true;
            try
            {
                _connection.Dispose();
            }
            catch (IOException  ex) {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying connect");
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), (ex, time) =>
                {
                    _logger.LogInformation(ex, $"RabbitMq Client could not connect after{time}s ({ex})", $"{time.TotalSeconds:n1}", ex.Message);
                });
                policy.Execute(()=>{

                    _connection = _connectionFactory.CreateConnection();
                });
                if(IsConnected)
                {
                    _connection.ConnectionShutdown +=OnConnectionShutDown;
                    _connection.CallbackException+= OnCallbackException;
                    _connection.ConnectionBlocked+= OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ client acquired a persistence connection to '{HostName}' and is subscribed to failure events",_connection.IsOpen);
                    return true;
                }
                else{
                    _logger.LogInformation("FATAL ERROR: RabbirMQ connections could not be created and opened");
                    return false;
                
                }
                
        }

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection is shutdown. Trying to re-connect");
            TryConnect();
        }

        private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection is shutdown. Trying to re-connect");
            TryConnect();
        }

        private void OnConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection is shutdown. Trying to re-connect");
            TryConnect();
        }
    }
}
