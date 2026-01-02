# SyZero.Consul

SyZero 框架的 Consul 服务注册与发现模块。

## 📦 安装

```bash
dotnet add package SyZero.Consul
```

## ✨ 特性

- 🚀 **服务注册** - 自动注册服务到 Consul
- 🔍 **服务发现** - 从 Consul 发现可用服务
- 💓 **健康检查** - 内置健康检查支持
- ⚙️ **配置中心** - 支持从 Consul KV 读取配置

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Consul": {
    "ConsulAddress": "http://localhost:8500",
    "Token": "",
    "ServiceName": "my-service",
    "ServiceAddress": "localhost",
    "ServicePort": 5000,
    "HealthCheckUrl": "/health"
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
builder.Services.AddConsul();

// 注册服务方式2 - 使用委托配置
builder.Services.AddConsul(options =>
{
    options.ConsulAddress = "http://localhost:8500";
    options.ServiceName = "my-service";
    options.ServicePort = 5000;
});

// 注册服务方式3 - 指定配置节
builder.Services.AddConsul(builder.Configuration, "Consul");

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class MyService
{
    private readonly IServiceManagement _serviceManagement;

    public MyService(IServiceManagement serviceManagement)
    {
        _serviceManagement = serviceManagement;
    }

    public async Task<string> GetServiceUrlAsync(string serviceName)
    {
        var service = await _serviceManagement.GetServiceAsync(serviceName);
        return $"{service.Address}:{service.Port}";
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `ConsulAddress` | `string` | `""` | Consul 服务地址 |
| `Token` | `string` | `""` | Consul ACL Token |
| `ServiceName` | `string` | `""` | 服务名称 |
| `ServiceAddress` | `string` | `""` | 服务地址 |
| `ServicePort` | `int` | `0` | 服务端口 |
| `HealthCheckUrl` | `string` | `"/health"` | 健康检查地址 |

---

## 📖 API 说明

### IServiceManagement 接口

| 方法 | 说明 |
|------|------|
| `GetServiceAsync(serviceName)` | 获取服务实例 |
| `GetServicesAsync(serviceName)` | 获取所有服务实例 |
| `RegisterAsync()` | 注册服务 |
| `DeregisterAsync()` | 注销服务 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 从 Consul KV 读取配置

```csharp
builder.Configuration.AddConsulConfiguration(options =>
{
    options.Address = "http://localhost:8500";
    options.Key = "config/my-service";
});
```

### 服务健康检查

```csharp
app.MapHealthChecks("/health");
```

---

## ⚠️ 注意事项

1. **网络连接** - 确保应用能访问 Consul 服务
2. **健康检查** - 必须配置健康检查端点
3. **服务注销** - 应用退出时会自动注销服务

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
