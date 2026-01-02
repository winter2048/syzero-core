# SyZero.Feign

SyZero 框架的声明式 HTTP 客户端模块，基于 Refit 实现类似 Spring Cloud OpenFeign 的功能。

## 📦 安装

```bash
dotnet add package SyZero.Feign
```

## ✨ 特性

- 🚀 **声明式调用** - 使用接口定义 HTTP 客户端
- 💾 **服务发现** - 集成服务注册与发现
- 🔒 **负载均衡** - 内置负载均衡支持
- 🔄 **gRPC 支持** - 同时支持 HTTP 和 gRPC 调用

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Feign": {
    "Timeout": 30000,
    "RetryCount": 3
  }
}
```

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
// 添加SyZero
builder.AddSyZero();

// 注册服务方式1 - 使用默认配置
builder.Services.AddSyZeroFeign();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroFeign(options =>
{
    options.Timeout = 30000;
    options.RetryCount = 3;
});

// 注册服务方式3 - 注册特定客户端
builder.Services.AddSyZeroFeign()
    .AddClient<IUserServiceClient>("user-service");

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
[FeignClient("user-service")]
public interface IUserServiceClient
{
    [Get("/api/user/{id}")]
    Task<UserDto> GetUserAsync(long id);

    [Post("/api/user")]
    Task<UserDto> CreateUserAsync([Body] CreateUserInput input);
}

public class MyService
{
    private readonly IUserServiceClient _userClient;

    public MyService(IUserServiceClient userClient)
    {
        _userClient = userClient;
    }

    public async Task<UserDto> GetUserAsync(long id)
    {
        return await _userClient.GetUserAsync(id);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Timeout` | `int` | `30000` | 请求超时时间（毫秒） |
| `RetryCount` | `int` | `3` | 重试次数 |
| `BaseUrl` | `string` | `""` | 基础地址（不使用服务发现时） |

---

## 📖 API 说明

### FeignClient 特性

| 属性 | 说明 |
|------|------|
| `Name` | 服务名称（用于服务发现） |
| `Url` | 固定地址（不使用服务发现） |
| `FallbackType` | 降级处理类型 |

> 使用特性标记接口方法的 HTTP 方法和路径

---

## 🔧 高级用法

### 请求拦截器

```csharp
builder.Services.AddSyZeroFeign()
    .AddInterceptor<AuthHeaderInterceptor>();

public class AuthHeaderInterceptor : IRequestInterceptor
{
    public Task InterceptAsync(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", "Bearer xxx");
        return Task.CompletedTask;
    }
}
```

### gRPC 调用

```csharp
[FeignClient("user-service", Protocol = Protocol.Grpc)]
public interface IUserGrpcClient
{
    Task<UserDto> GetUserAsync(long id);
}
```

---

## ⚠️ 注意事项

1. **服务发现** - 使用服务名时需要配置服务发现组件
2. **超时配置** - 根据业务合理配置超时时间
3. **降级处理** - 建议实现 Fallback 以提高可用性

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
