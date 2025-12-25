# SyZero.DynamicGrpc

动态 gRPC 服务生成框架，基于 `protobuf-net.Grpc` 实现 **Code-First gRPC**，无需 .proto 文件。

## 📦 安装

```bash
dotnet add package SyZero.DynamicGrpc
```

## ✨ 特性

- 🎯 **Code-First** - 无需 .proto 文件，直接从 C# 接口生成 gRPC 服务
- 🚀 **自动服务发现** - 基于 `IDynamicApi` 接口和特性标记自动发现 gRPC 服务
- ⚡ **高性能** - 使用 Protobuf 二进制序列化，性能优异
- 🔧 **灵活配置** - 支持自定义消息大小限制、详细错误等
- 🎯 **无侵入设计** - 与现有 `SyZero` 框架无缝集成

---

## 🚀 快速开始（Code-First 模式）

### 1. 定义数据契约

直接使用普通 POCO 类：

```csharp
public class HelloRequest
{
    public string Name { get; set; }
}

public class HelloReply
{
    public string Message { get; set; }
}
```

### 2. 定义服务接口

```csharp
using System.Threading.Tasks;
using SyZero.Application.Service;
using SyZero.Application.Attributes;

[DynamicApi]  // 标记在接口层，自动注册为 gRPC 服务
public interface IGreeterService : IApplicationService, IDynamicApi
{
    Task<HelloReply> SayHello(HelloRequest request);
    
    Task<HelloReply> SayGoodbye(HelloRequest request);
}
```

### 3. 实现服务

```csharp
public class GreeterService : IGreeterService
{
    public Task<HelloReply> SayHello(HelloRequest request)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello, {request.Name}!"
        });
    }

    public Task<HelloReply> SayGoodbye(HelloRequest request)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Goodbye, {request.Name}!"
        });
    }
}
```

### 4. 配置服务端

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 Dynamic gRPC 服务
builder.Services.AddDynamicGrpc(options =>
{
    options.MaxReceiveMessageSize = 10 * 1024 * 1024; // 10MB
    options.MaxSendMessageSize = 10 * 1024 * 1024;    // 10MB
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

var app = builder.Build();

// 映射 gRPC 服务端点
app.MapDynamicGrpcServices();

app.Run();
```

### 5. 客户端调用

```csharp
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

// 创建通道
using var channel = GrpcChannel.ForAddress("https://localhost:5001");

// 创建客户端（Code-First 方式）
var client = channel.CreateGrpcService<IGreeterService>();

// 调用服务
var reply = await client.SayHello(new HelloRequest { Name = "World" });
Console.WriteLine(reply.Message); // Hello, World!
```

---

## 📖 配置选项

### DynamicGrpcOptions

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `RemoveServicePostfixes` | `List<string>` | `["Service", "AppService", ...]` | 移除服务名称后缀 |
| `RemoveMethodPostfixes` | `List<string>` | `["Async"]` | 移除方法名称后缀 |
| `MaxReceiveMessageSize` | `int?` | `null` | 最大接收消息大小（字节） |
| `MaxSendMessageSize` | `int?` | `null` | 最大发送消息大小（字节） |
| `EnableDetailedErrors` | `bool` | `false` | 启用详细错误信息 |

### 配置示例

```csharp
builder.Services.AddDynamicGrpc(options =>
{
    // 消息大小限制
    options.MaxReceiveMessageSize = 20 * 1024 * 1024; // 20MB
    options.MaxSendMessageSize = 20 * 1024 * 1024;    // 20MB
    
    // 开发环境启用详细错误
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});
```

---

## 🏷️ 特性标记

> **说明**：标记了 `[DynamicApi]` 的服务默认会自动注册为 gRPC 服务，无需额外标记。

### NonGrpcServiceAttribute

排除某个 DynamicApi 服务不注册为 gRPC 服务：

```csharp
[DynamicApi]
[NonGrpcService]  // 排除此服务不注册为 gRPC
public interface IInternalService : IApplicationService, IDynamicApi
{
    // 此服务只会注册为 HTTP API，不会注册为 gRPC 服务
}
```

### NonGrpcMethodAttribute

排除某个方法不作为 gRPC 方法：

```csharp
[DynamicApi]
public interface IMyService : IApplicationService, IDynamicApi
{
    Task<Response> NormalMethod(Request request);
    
    [NonGrpcMethod]
    void InternalMethod();  // 此方法不会暴露为 gRPC
}
```

---

## 🔄 流式传输支持

### 服务端流

```csharp
public interface IStreamService : IDynamicApi
{
    IAsyncEnumerable<DataItem> GetDataStream(DataRequest request);
}
```

### 客户端流

```csharp
public interface IStreamService : IDynamicApi
{
    Task<DataResponse> UploadData(IAsyncEnumerable<DataItem> items);
}
```

### 双向流

```csharp
public interface IStreamService : IDynamicApi
{
    IAsyncEnumerable<DataItem> ProcessData(IAsyncEnumerable<DataItem> items);
}
```

---

## 🔗 与 DynamicWebApi 集成

同一服务同时支持 HTTP REST 和 gRPC：

```csharp
[DynamicApi]  // 标记在接口层，同时暴露为 HTTP API 和 gRPC 服务
public interface IUserService : IApplicationService, IDynamicApi
{
    Task<UserResponse> GetUser(UserRequest request);
}

public class UserService : IUserService
{
    public Task<UserResponse> GetUser(UserRequest request)
    {
        return Task.FromResult(new UserResponse { Id = request.Id, Name = "John" });
    }
}
```

```csharp
// Program.cs
builder.Services.AddDynamicWebApi();  // HTTP API
builder.Services.AddDynamicGrpc();    // gRPC

var app = builder.Build();

app.MapControllers();          // HTTP 端点
app.MapDynamicGrpcServices();  // gRPC 端点
```

---

## 🛡️ 拦截器

```csharp
public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("gRPC: {Method}", context.Method);
        return await continuation(request, context);
    }
}
```

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
