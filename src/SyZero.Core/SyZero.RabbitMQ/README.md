# SyZero.RabbitMQ

基于 RabbitMQ 的事件总线实现，为 SyZero 框架提供分布式消息传递能力。

## 功能特性

- ✅ 完整实现 `IEventBus` 接口
- ✅ 支持事件订阅/取消订阅
- ✅ 支持动态事件处理
- ✅ 自动重连机制
- ✅ 消息持久化
- ✅ 死信队列支持
- ✅ 批量发布事件
- ✅ 消息重试策略
- ✅ 连接池管理

## 安装

```bash
dotnet add package SyZero.RabbitMQ
```

## 快速开始

### 1. 配置服务

```csharp
using SyZero;

// 方式1: 代码配置
var options = new SyZero.RabbitMQ.RabbitMQEventBusOptions
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest",
    ExchangeName = "my_event_bus",
    QueueNamePrefix = "my_app"
};
services.AddRabbitMQEventBus(options);

// 方式2: 从配置文件读取
services.AddRabbitMQEventBus();

// 方式3: 从配置文件读取 + 额外配置
services.AddRabbitMQEventBus(options =>
{
    options.RetryCount = 5;
    options.EnableDeadLetter = true;
});

// 方式4: 指定配置节名称
services.AddRabbitMQEventBus(configuration: Configuration, sectionName: "MyRabbitMQ");
```

### 2. appsettings.json 配置

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "ExchangeName": "syzero_event_bus",
    "ExchangeType": "topic",
    "QueueNamePrefix": "syzero",
    "RetryCount": 3,
    "PrefetchCount": 1,
    "EnableDeadLetter": true
  }
}
```

### 3. 定义事件

```csharp
using SyZero.EventBus;

public class OrderCreatedEvent : EventBase
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime OrderTime { get; set; }
}
```

### 4. 定义事件处理器

```csharp
using SyZero.EventBus;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        _logger.LogInformation($"处理订单创建事件: {@event.OrderId}");
        
        // 处理业务逻辑
        await Task.CompletedTask;
    }
}
```

### 5. 订阅和发布事件

```csharp
public class OrderService
{
    private readonly IEventBus _eventBus;

    public OrderService(IEventBus eventBus)
    {
        _eventBus = eventBus;
        
        // 订阅事件
        _eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>(
            () => new OrderCreatedEventHandler(logger));
    }

    public async Task CreateOrderAsync(Order order)
    {
        // 创建订单逻辑
        
        // 发布事件
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            Amount = order.Amount,
            OrderTime = DateTime.Now
        };
        
        await _eventBus.PublishAsync(orderEvent);
    }
}
```

## 配置选项

| 选项 | 说明 | 默认值 |
|-----|------|--------|
| HostName | RabbitMQ 主机地址 | localhost |
| Port | 端口号 | 5672 |
| UserName | 用户名 | guest |
| Password | 密码 | guest |
| VirtualHost | 虚拟主机 | / |
| ExchangeName | 交换机名称 | syzero_event_bus |
| ExchangeType | 交换机类型 | topic |
| QueueNamePrefix | 队列名称前缀 | syzero |
| RetryCount | 重试次数 | 3 |
| RetryIntervalMilliseconds | 重试间隔(ms) | 1000 |
| PrefetchCount | 预取数量 | 1 |
| QueueDurable | 队列持久化 | true |
| MessagePersistent | 消息持久化 | true |
| EnableDeadLetter | 启用死信队列 | true |
| MessageTTL | 消息TTL(ms) | null |
| MaxLength | 最大消息长度 | null |

## 高级用法

### 动态事件处理

```csharp
public class DynamicEventHandler : IDynamicEventHandler
{
    public async Task HandleAsync(string eventName, dynamic eventData)
    {
        Console.WriteLine($"处理动态事件: {eventName}");
        Console.WriteLine($"事件数据: {eventData}");
        await Task.CompletedTask;
    }
}

// 订阅动态事件
_eventBus.SubscribeDynamic<DynamicEventHandler>("CustomEvent");

// 发布动态事件
await _eventBus.PublishAsync("CustomEvent", new { Data = "test" });
```

### 批量发布事件

```csharp
var events = new List<EventBase>
{
    new OrderCreatedEvent { OrderId = "1" },
    new OrderCreatedEvent { OrderId = "2" },
    new OrderCreatedEvent { OrderId = "3" }
};

await _eventBus.PublishBatchAsync(events);
```

## 最佳实践

1. **连接管理**：使用单例模式注册 EventBus，避免重复创建连接
2. **消息持久化**：生产环境建议开启消息持久化
3. **死信队列**：启用死信队列处理失败消息
4. **预取数量**：根据消费者处理能力调整 PrefetchCount
5. **重试策略**：合理设置重试次数和间隔
6. **日志监控**：关注连接状态和消息处理异常

## 注意事项

- RabbitMQ 服务器需要正常运行
- 确保网络连接稳定
- 消息序列化使用 System.Text.Json
- 支持自动重连和故障恢复
- 依赖 Polly 库实现重试策略

## 依赖项

- RabbitMQ.Client >= 6.8.1
- Microsoft.Extensions.DependencyInjection.Abstractions >= 8.0.0
- Microsoft.Extensions.Logging.Abstractions >= 8.0.0
- Polly >= 8.0.0

## 许可证

MIT License
