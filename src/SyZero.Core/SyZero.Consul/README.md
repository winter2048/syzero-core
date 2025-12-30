# SyZero.Consul

基于 Consul 的服务注册、发现和配置中心集成组件。

## 📦 安装

```bash
dotnet add package SyZero.Consul
```

## ✨ 特性

- 🚀 **服务注册** - 自动将服务注册到 Consul，支持健康检查
- 🔍 **服务发现** - 从 Consul 获取服务列表，支持缓存优化
- ⚙️ **配置中心** - 从 Consul KV 存储读取配置，支持热更新
- 🔄 **自动注销** - 应用程序停止时自动注销服务
- 🛡️ **高可用** - 支持 ACL Token 认证

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Server": {
    "Name": "my-service",
    "WanIp": "192.168.1.100",
    "Port": 5000,
    "Protocol": "HTTP",
    "InspectInterval": 10
  },
  "Consul": {
    "ConsulAddress": "http://localhost:8500",
    "HealthCheck": "/health",
    "Token": ""
  }
}
```

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 方式一：从 AppConfig 读取配置（默认从 appsettings.json 的 "Consul" 节点读取）
builder.Services.AddConsul();

// 方式二：从 IConfiguration 读取配置
builder.Services.AddConsul(builder.Configuration);

// 方式三：从 IConfiguration 读取配置，并支持额外代码配置
builder.Services.AddConsul(builder.Configuration, options =>
{
    options.Token = "your-acl-token";
});

// 方式四：使用配置委托
builder.Services.AddConsul(options =>
{
    options.ConsulAddress = "http://localhost:8500";
    options.HealthCheck = "/health";
});

// 方式五：直接传入配置对象
builder.Services.AddConsul(new ConsulServiceOptions
{
    ConsulAddress = "http://localhost:8500",
    HealthCheck = "/health"
});

var app = builder.Build();

// 使用 SyZero（会自动注册服务到 Consul）
app.UseSyZero();

app.Run();
```

### 3. 健康检查端点

确保添加健康检查端点：

```csharp
app.MapGet("/health", () => Results.Ok("Healthy"));
```

---

## 📖 配置选项

### ConsulServiceOptions

| 属性 | 类型 | 说明 |
|------|------|------|
| `ConsulAddress` | `string` | Consul 服务地址 |
| `HealthCheck` | `string` | 健康检查路径 |
| `Token` | `string` | ACL Token（可选） |
| `ServiceId` | `string` | 服务 ID（自动生成） |

### Server 配置

| 属性 | 类型 | 说明 |
|------|------|------|
| `Name` | `string` | 服务名称 |
| `WanIp` | `string` | 服务 IP 地址 |
| `Port` | `int` | 服务端口 |
| `Protocol` | `string` | 协议类型（HTTP/HTTPS/GRPC） |
| `InspectInterval` | `int` | 健康检查间隔（秒） |

---

## 🔍 服务发现

### 使用 IServiceManagement

```csharp
public class MyService
{
    private readonly IServiceManagement _serviceManagement;

    public MyService(IServiceManagement serviceManagement)
    {
        _serviceManagement = serviceManagement;
    }

    public async Task CallOtherService()
    {
        // 获取服务列表
        var services = await _serviceManagement.GetService("other-service");
        
        // 选择一个服务实例
        var service = services.First();
        
        // 构建请求地址
        var url = $"{service.ServiceProtocol}://{service.ServiceAddress}:{service.ServicePort}/api/endpoint";
    }
}
```

### ServiceInfo 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `ServiceID` | `string` | 服务实例 ID |
| `ServiceName` | `string` | 服务名称 |
| `ServiceAddress` | `string` | 服务地址 |
| `ServicePort` | `int` | 服务端口 |
| `ServiceProtocol` | `ProtocolType` | 协议类型 |

---

## ⚙️ 配置中心

### 从 Consul KV 读取配置

```csharp
var builder = WebApplication.CreateBuilder(args);

// 添加 Consul 配置源
builder.Configuration.AddConsul(cancellationToken);

var app = builder.Build();
```

### 自定义配置源

```csharp
builder.Configuration.AddConsul("my-service-config", cancellationToken, source =>
{
    source.ConsulClientConfiguration = config =>
    {
        config.Address = new Uri("http://localhost:8500");
        config.Token = "your-acl-token";
    };
    source.Optional = true;
    source.ReloadOnChange = true;  // 启用配置热更新
    source.ReloadDelay = 300;      // 重新加载延迟（毫秒）
});
```

### 配置热更新

当 `ReloadOnChange = true` 时，配置变更会自动重新加载：

```csharp
// 使用 IOptionsSnapshot 获取最新配置
public class MyService
{
    private readonly IOptionsSnapshot<MyOptions> _options;

    public MyService(IOptionsSnapshot<MyOptions> options)
    {
        _options = options;
    }

    public void DoSomething()
    {
        var currentValue = _options.Value.SomeSetting;
    }
}
```

---

## 🔒 gRPC 服务支持

对于 gRPC 服务，健康检查会自动使用 gRPC 协议：

```json
{
  "Server": {
    "Name": "my-grpc-service",
    "WanIp": "192.168.1.100",
    "Port": 5001,
    "Protocol": "GRPC",
    "InspectInterval": 10
  }
}
```

---

## 🔗 与其他组件集成

### 与 SyZero.Feign 集成

```csharp
// 自动从 Consul 发现服务并调用
[FeignClient("other-service")]
public interface IOtherServiceClient
{
    [Get("/api/users/{id}")]
    Task<User> GetUser(long id);
}
```

### 与 SyZero.DynamicGrpc 集成

```csharp
// gRPC 服务自动注册到 Consul
builder.Services.AddDynamicGrpc();
builder.Services.AddConsul();

var app = builder.Build();

app.MapDynamicGrpcServices();
app.UseSyZero();
```

---

## ⚠️ 注意事项

1. **健康检查** - 确保配置的健康检查端点可访问
2. **网络** - 确保服务与 Consul 之间网络畅通
3. **ACL Token** - 生产环境建议配置 ACL Token
4. **缓存** - 服务发现结果会缓存 30 秒，减少 Consul 压力

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
