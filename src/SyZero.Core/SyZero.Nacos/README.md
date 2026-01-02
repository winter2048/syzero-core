# SyZero.Nacos

SyZero 框架的 Nacos 服务注册与配置中心模块。

## 📦 安装

```bash
dotnet add package SyZero.Nacos
```

## ✨ 特性

- 🚀 **服务注册** - 自动注册服务到 Nacos
- 🔍 **服务发现** - 从 Nacos 发现可用服务
- ⚙️ **配置中心** - 从 Nacos 读取和监听配置变更
- 💓 **健康检查** - 内置心跳和健康检查

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Nacos": {
    "ServerAddresses": ["http://localhost:8848"],
    "Namespace": "public",
    "ServiceName": "my-service",
    "GroupName": "DEFAULT_GROUP",
    "ClusterName": "DEFAULT",
    "Ip": "localhost",
    "Port": 5000
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
builder.Configuration.AddNacosConfiguration(builder.Configuration);
builder.Services.AddNacos();

// 注册服务方式2 - 使用委托配置
builder.Services.AddNacos(options =>
{
    options.ServerAddresses = new[] { "http://localhost:8848" };
    options.ServiceName = "my-service";
    options.Port = 5000;
});

// 注册服务方式3 - 添加配置中心
builder.Configuration.AddNacosConfiguration(options =>
{
    options.DataId = "my-service-config";
    options.Group = "DEFAULT_GROUP";
});

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
| `ServerAddresses` | `string[]` | `[]` | Nacos 服务地址列表 |
| `Namespace` | `string` | `"public"` | 命名空间 |
| `ServiceName` | `string` | `""` | 服务名称 |
| `GroupName` | `string` | `"DEFAULT_GROUP"` | 分组名称 |
| `ClusterName` | `string` | `"DEFAULT"` | 集群名称 |

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

### 监听配置变更

```csharp
builder.Configuration.AddNacosConfiguration(options =>
{
    options.DataId = "my-service-config";
    options.OnConfigChanged = (config) =>
    {
        Console.WriteLine("配置已更新");
    };
});
```

### 元数据管理

```csharp
builder.Services.AddNacos(options =>
{
    options.Metadata = new Dictionary<string, string>
    {
        ["version"] = "1.0.0",
        ["env"] = "production"
    };
});
```

---

## ⚠️ 注意事项

1. **网络连接** - 确保应用能访问 Nacos 服务
2. **命名空间** - 不同环境使用不同命名空间隔离
3. **心跳** - 服务会自动发送心跳保持注册状态

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
