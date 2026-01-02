# SyZero.RabbitMQ

SyZero 框架的 RabbitMQ 事件总线模块，提供分布式消息队列支持。

## 📦 安装

```bash
dotnet add package SyZero.RabbitMQ
```

## ✨ 特性

- 🚀 **事件总线** - 基于 RabbitMQ 的分布式事件总线
- 💾 **持久化** - 消息持久化保证可靠性
- 🔄 **自动重连** - 连接断开后自动重连
- 📨 **发布订阅** - 支持发布/订阅模式

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "ExchangeName": "my_exchange",
    "QueueName": "my_queue"
  }
}
```

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
// 添加SyZero
builder.AddSyZero();

// 注册服务方式1 - 使用配置文件
builder.Services.AddRabbitMQEventBus();

// 注册服务方式2 - 使用委托配置
builder.Services.AddRabbitMQEventBus(options =>
{
    options.HostName = "localhost";
    options.Port = 5672;
    options.UserName = "guest";
    options.Password = "guest";
});

// 注册服务方式3 - 指定配置节
builder.Services.AddRabbitMQEventBus(builder.Configuration, "RabbitMQ");

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
// 定义事件
public class UserCreatedEvent : IEvent
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}

// 发布事件
public class UserService
{
    private readonly IEventBus _eventBus;

    public UserService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreateUserAsync(User user)
    {
        // 创建用户后发布事件
        await _eventBus.PublishAsync(new UserCreatedEvent
        {
            UserId = user.Id,
            UserName = user.Name
        });
    }
}

// 订阅事件
public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent @event)
    {
        Console.WriteLine($"用户 {@event.UserName} 创建成功");
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `HostName` | `string` | `"localhost"` | RabbitMQ 主机地址 |
| `Port` | `int` | `5672` | 端口号 |
| `UserName` | `string` | `"guest"` | 用户名 |
| `Password` | `string` | `"guest"` | 密码 |
| `VirtualHost` | `string` | `"/"` | 虚拟主机 |
| `ExchangeName` | `string` | `""` | 交换机名称 |
| `QueueName` | `string` | `""` | 队列名称 |
| `RetryCount` | `int` | `5` | 重试次数 |

---

## 📖 API 说明

### IEventBus 接口

| 方法 | 说明 |
|------|------|
| `PublishAsync<TEvent>(event)` | 发布事件 |
| `Subscribe<TEvent, THandler>()` | 订阅事件 |
| `Unsubscribe<TEvent, THandler>()` | 取消订阅 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 延迟消息

```csharp
await _eventBus.PublishAsync(new OrderTimeoutEvent
{
    OrderId = orderId
}, delay: TimeSpan.FromMinutes(30));
```

### 死信队列

```csharp
builder.Services.AddRabbitMQEventBus(options =>
{
    options.DeadLetterExchange = "dead_letter_exchange";
    options.DeadLetterQueue = "dead_letter_queue";
});
```

---

## ⚠️ 注意事项

1. **连接管理** - 应用会自动管理连接和重连
2. **消息确认** - 默认使用手动确认模式
3. **错误处理** - 处理失败的消息会进入死信队列

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
