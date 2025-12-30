using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SyZero.EventBus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Polly;

namespace SyZero.RabbitMQ
{
    /// <summary>
    /// RabbitMQ 事件总线实现
    /// </summary>
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly RabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQEventBusOptions _options;
        private readonly ConcurrentDictionary<string, List<Type>> _subscriptions;
        private readonly ConcurrentDictionary<string, List<Type>> _dynamicSubscriptions;
        private readonly ConcurrentDictionary<string, Func<object>> _handlerFactories;
        private IModel _consumerChannel;
        private string _queueName;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RabbitMQEventBus(
            RabbitMQPersistentConnection persistentConnection,
            ILogger<RabbitMQEventBus> logger,
            IServiceProvider serviceProvider,
            RabbitMQEventBusOptions options)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _subscriptions = new ConcurrentDictionary<string, List<Type>>();
            _dynamicSubscriptions = new ConcurrentDictionary<string, List<Type>>();
            _handlerFactories = new ConcurrentDictionary<string, Func<object>>();

            _queueName = $"{_options.QueueNamePrefix}_{Guid.NewGuid()}";
            _consumerChannel = CreateConsumerChannel();
        }

        #region 订阅管理

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void Subscribe<T, TH>(Func<TH> handler)
            where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            DoSubscribe(typeof(TH), eventName, () => handler());
            StartBasicConsume();
        }

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            DoSubscribeDynamic(typeof(TH), eventName);
            StartBasicConsume();
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public void Unsubscribe<T, TH>()
            where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            var handlerType = typeof(TH);

            if (_subscriptions.ContainsKey(eventName))
            {
                _subscriptions[eventName].Remove(handlerType);
                _handlerFactories.TryRemove($"{eventName}_{handlerType.Name}", out _);

                if (_subscriptions[eventName].Count == 0)
                {
                    _subscriptions.TryRemove(eventName, out _);
                    RemoveBinding(eventName);
                }

                _logger.LogInformation($"事件处理器 {handlerType.Name} 取消订阅事件 {eventName}");
            }
        }

        /// <summary>
        /// 取消订阅动态事件
        /// </summary>
        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            var handlerType = typeof(TH);

            if (_dynamicSubscriptions.ContainsKey(eventName))
            {
                _dynamicSubscriptions[eventName].Remove(handlerType);

                if (_dynamicSubscriptions[eventName].Count == 0)
                {
                    _dynamicSubscriptions.TryRemove(eventName, out _);
                    RemoveBinding(eventName);
                }

                _logger.LogInformation($"动态事件处理器 {handlerType.Name} 取消订阅事件 {eventName}");
            }
        }

        #endregion

        #region 发布事件

        /// <summary>
        /// 发布事件
        /// </summary>
        public void Publish(EventBase @event)
        {
            PublishAsync(@event).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步发布事件
        /// </summary>
        public async Task PublishAsync(EventBase @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventName = @event.GetType().Name;
            await PublishAsync(eventName, @event);
        }

        /// <summary>
        /// 发布事件（指定事件名称）
        /// </summary>
        public void Publish(string eventName, object eventData)
        {
            PublishAsync(eventName, eventData).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步发布事件（指定事件名称）
        /// </summary>
        public async Task PublishAsync(string eventName, object eventData)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = CreateRetryPolicy();

            await policy.ExecuteAsync(async () =>
            {
                using var channel = _persistentConnection.CreateModel();

                // 声明交换机
                channel.ExchangeDeclare(
                    exchange: _options.ExchangeName,
                    type: _options.ExchangeType,
                    durable: true);

                // 序列化事件数据
                var message = JsonSerializer.Serialize(eventData, new JsonSerializerOptions
                {
                    WriteIndented = false
                });
                var body = Encoding.UTF8.GetBytes(message);

                // 设置消息属性
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = _options.MessagePersistent ? (byte)2 : (byte)1;
                properties.ContentType = "application/json";
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                // 发布消息
                channel.BasicPublish(
                    exchange: _options.ExchangeName,
                    routingKey: eventName,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation($"已发布事件 {eventName} 到 RabbitMQ");

                await Task.CompletedTask;
            });
        }

        /// <summary>
        /// 批量发布事件
        /// </summary>
        public void PublishBatch(IEnumerable<EventBase> events)
        {
            PublishBatchAsync(events).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步批量发布事件
        /// </summary>
        public async Task PublishBatchAsync(IEnumerable<EventBase> events)
        {
            if (events == null || !events.Any())
                return;

            foreach (var @event in events)
            {
                await PublishAsync(@event);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 清空所有订阅
        /// </summary>
        public void Clear()
        {
            _subscriptions.Clear();
            _dynamicSubscriptions.Clear();
            _handlerFactories.Clear();
            _logger.LogInformation("已清空所有订阅");
        }

        /// <summary>
        /// 检查是否已订阅事件
        /// </summary>
        public bool IsSubscribed<T>()
        {
            var eventName = GetEventKey<T>();
            return IsSubscribed(eventName);
        }

        /// <summary>
        /// 检查是否已订阅事件（通过事件名称）
        /// </summary>
        public bool IsSubscribed(string eventName)
        {
            return _subscriptions.ContainsKey(eventName) || _dynamicSubscriptions.ContainsKey(eventName);
        }

        /// <summary>
        /// 获取所有订阅的事件名称
        /// </summary>
        public IEnumerable<string> GetSubscribedEvents()
        {
            var subscribedEvents = new HashSet<string>();
            
            foreach (var eventName in _subscriptions.Keys)
            {
                subscribedEvents.Add(eventName);
            }
            
            foreach (var eventName in _dynamicSubscriptions.Keys)
            {
                subscribedEvents.Add(eventName);
            }
            
            return subscribedEvents;
        }

        private void DoSubscribe(Type handlerType, string eventName, Func<object> handlerFactory)
        {
            if (!_subscriptions.ContainsKey(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using var channel = _persistentConnection.CreateModel();
                channel.QueueBind(
                    queue: _queueName,
                    exchange: _options.ExchangeName,
                    routingKey: eventName);

                _subscriptions[eventName] = new List<Type>();
            }

            if (_subscriptions[eventName].Contains(handlerType))
            {
                _logger.LogWarning($"事件处理器 {handlerType.Name} 已订阅事件 {eventName}");
                return;
            }

            _subscriptions[eventName].Add(handlerType);
            _handlerFactories[$"{eventName}_{handlerType.Name}"] = handlerFactory;

            _logger.LogInformation($"事件处理器 {handlerType.Name} 订阅事件 {eventName}");
        }

        private void DoSubscribeDynamic(Type handlerType, string eventName)
        {
            if (!_dynamicSubscriptions.ContainsKey(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using var channel = _persistentConnection.CreateModel();
                channel.QueueBind(
                    queue: _queueName,
                    exchange: _options.ExchangeName,
                    routingKey: eventName);

                _dynamicSubscriptions[eventName] = new List<Type>();
            }

            if (_dynamicSubscriptions[eventName].Contains(handlerType))
            {
                _logger.LogWarning($"动态事件处理器 {handlerType.Name} 已订阅事件 {eventName}");
                return;
            }

            _dynamicSubscriptions[eventName].Add(handlerType);

            _logger.LogInformation($"动态事件处理器 {handlerType.Name} 订阅事件 {eventName}");
        }

        private void RemoveBinding(string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            channel.QueueUnbind(
                queue: _queueName,
                exchange: _options.ExchangeName,
                routingKey: eventName);

            _logger.LogInformation($"已移除事件 {eventName} 的绑定");
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogInformation("正在创建 RabbitMQ 消费者通道");

            var channel = _persistentConnection.CreateModel();

            // 声明交换机
            channel.ExchangeDeclare(
                exchange: _options.ExchangeName,
                type: _options.ExchangeType,
                durable: true);

            // 声明死信交换机（如果启用）
            if (_options.EnableDeadLetter)
            {
                channel.ExchangeDeclare(
                    exchange: _options.DeadLetterExchangeName,
                    type: _options.ExchangeType,
                    durable: true);
            }

            // 声明队列
            var queueArgs = new Dictionary<string, object>();
            if (_options.EnableDeadLetter)
            {
                queueArgs["x-dead-letter-exchange"] = _options.DeadLetterExchangeName;
            }
            if (_options.MessageTTL.HasValue)
            {
                queueArgs["x-message-ttl"] = _options.MessageTTL.Value;
            }
            if (_options.MaxLength.HasValue)
            {
                queueArgs["x-max-length"] = _options.MaxLength.Value;
            }

            channel.QueueDeclare(
                queue: _queueName,
                durable: _options.QueueDurable,
                exclusive: false,
                autoDelete: _options.QueueAutoDelete,
                arguments: queueArgs);

            // 设置预取数量
            channel.BasicQos(0, _options.PrefetchCount, false);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "重新创建 RabbitMQ 消费者通道");
                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            _logger.LogInformation("开始 RabbitMQ 基本消费");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += async (model, ea) =>
                {
                    var eventName = ea.RoutingKey;
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    try
                    {
                        await ProcessEvent(eventName, message);
                        _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"处理事件 {eventName} 时发生错误: {message}");
                        
                        // 拒绝消息并重新入队
                        _consumerChannel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                    }
                };

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: _options.AutoAck,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume 无法调用，_consumerChannel 为 null");
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogInformation($"处理事件: {eventName}");

            // 处理普通订阅
            if (_subscriptions.ContainsKey(eventName))
            {
                var handlerTypes = _subscriptions[eventName];
                foreach (var handlerType in handlerTypes)
                {
                    var factoryKey = $"{eventName}_{handlerType.Name}";
                    if (_handlerFactories.TryGetValue(factoryKey, out var factory))
                    {
                        var handler = factory();
                        if (handler == null)
                        {
                            _logger.LogWarning($"无法创建处理器实例: {handlerType.Name}");
                            continue;
                        }

                        var eventType = _subscriptions[eventName].FirstOrDefault()?.BaseType?.GetGenericArguments()?.FirstOrDefault();
                        if (eventType != null)
                        {
                            var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }

            // 处理动态订阅
            if (_dynamicSubscriptions.ContainsKey(eventName))
            {
                var handlerTypes = _dynamicSubscriptions[eventName];
                using var scope = _serviceProvider.CreateScope();
                
                foreach (var handlerType in handlerTypes)
                {
                    var handler = scope.ServiceProvider.GetService(handlerType) as IDynamicEventHandler;
                    if (handler == null)
                    {
                        _logger.LogWarning($"无法创建动态处理器实例: {handlerType.Name}");
                        continue;
                    }

                    dynamic eventData = JsonSerializer.Deserialize<dynamic>(message);
                    await handler.HandleAsync(eventName, eventData);
                }
            }
        }

        private string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        private Polly.IAsyncPolicy CreateRetryPolicy()
        {
            return Polly.Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    _options.RetryCount,
                    retryAttempt => TimeSpan.FromMilliseconds(_options.RetryIntervalMilliseconds * retryAttempt),
                    (ex, time, retry, ctx) =>
                    {
                        _logger.LogWarning(ex, $"RabbitMQ 发布失败，重试 {retry}/{_options.RetryCount}，等待 {time.TotalMilliseconds}ms");
                    });
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _consumerChannel?.Dispose();
            _subscriptions?.Clear();
            _dynamicSubscriptions?.Clear();
            _handlerFactories?.Clear();

            _logger.LogInformation("RabbitMQ EventBus 已释放");
        }

        #endregion
    }
}
