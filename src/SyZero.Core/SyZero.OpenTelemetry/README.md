# SyZero.OpenTelemetry

SyZero 框架的 OpenTelemetry 可观测性模块，提供分布式追踪和指标收集。

## 📦 安装

```bash
dotnet add package SyZero.OpenTelemetry
```

## ✨ 特性

- 🚀 **分布式追踪** - 自动追踪 HTTP 请求和数据库调用
- 📊 **指标收集** - 收集应用性能指标
- 🔗 **链路追踪** - 跨服务调用链路追踪

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "OpenTelemetry": {
    "ServiceName": "my-service",
    "Endpoint": "http://localhost:4317",
    "EnableTracing": true,
    "EnableMetrics": true
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
builder.Services.AddSyZeroOpenTelemetry();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroOpenTelemetry(options =>
{
    options.ServiceName = "my-service";
    options.Endpoint = "http://localhost:4317";
});

// 注册服务方式3 - 自定义导出器
builder.Services.AddSyZeroOpenTelemetry(options =>
{
    options.ServiceName = "my-service";
    options.UseJaeger("http://localhost:14268/api/traces");
});

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class OrderService
{
    private readonly ITracer _tracer;

    public OrderService(ITracer tracer)
    {
        _tracer = tracer;
    }

    public async Task CreateOrderAsync(Order order)
    {
        using var span = _tracer.StartSpan("CreateOrder");
        span.SetAttribute("order.id", order.Id);
        
        // 业务逻辑
        
        span.SetStatus(Status.Ok);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `ServiceName` | `string` | `""` | 服务名称 |
| `Endpoint` | `string` | `""` | OTLP 端点地址 |
| `EnableTracing` | `bool` | `true` | 启用追踪 |
| `EnableMetrics` | `bool` | `true` | 启用指标 |

---

## 📖 API 说明

### ITracer 接口

| 方法 | 说明 |
|------|------|
| `StartSpan(name)` | 开始一个新的追踪 Span |
| `CurrentSpan` | 获取当前 Span |

> 自动追踪 HTTP 请求、数据库调用等

---

## 🔧 高级用法

### 自定义 Span

```csharp
using var span = _tracer.StartSpan("CustomOperation");
span.SetAttribute("key", "value");
span.AddEvent("Something happened");
```

### 添加 Baggage

```csharp
Baggage.SetBaggage("user.id", userId);
```

---

## ⚠️ 注意事项

1. **端点配置** - 确保 OTLP 端点可访问
2. **采样率** - 生产环境建议配置采样率
3. **性能影响** - 追踪会有轻微性能开销

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
