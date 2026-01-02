# SyZero.DynamicGrpc

SyZero 框架的动态 gRPC 模块，支持自动生成 gRPC 服务。

## 📦 安装

```bash
dotnet add package SyZero.DynamicGrpc
```

## ✨ 特性

- 🚀 **动态生成** - 根据应用服务自动生成 gRPC 服务
- 💾 **无需 Proto** - 无需手动编写 .proto 文件
- 🔒 **类型安全** - 保持完整的类型检查

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "DynamicGrpc": {
    "EnableReflection": true
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
builder.Services.AddDynamicGrpc();

// 注册服务方式2 - 使用委托配置
builder.Services.AddDynamicGrpc(options =>
{
    options.EnableReflection = true;
});

// 注册服务方式3 - 指定服务程序集
builder.Services.AddDynamicGrpc(typeof(UserAppService).Assembly);

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
// 映射 gRPC 服务
app.MapDynamicGrpcService();
app.Run();
```

### 3. 使用示例

```csharp
public interface IUserAppService : IApplicationService
{
    Task<UserDto> GetUserAsync(long id);
    Task<List<UserDto>> GetUsersAsync();
}

public class UserAppService : IUserAppService
{
    public async Task<UserDto> GetUserAsync(long id)
    {
        // 实现逻辑
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        // 实现逻辑
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `EnableReflection` | `bool` | `true` | 启用 gRPC 反射 |

---

## 📖 API 说明

### IApplicationService 接口

| 方法 | 说明 |
|------|------|
| 继承此接口的服务方法 | 自动暴露为 gRPC 方法 |

> 所有公开方法都会自动生成对应的 gRPC 服务方法

---

## 🔧 高级用法

### 自定义序列化

```csharp
builder.Services.AddDynamicGrpc(options =>
{
    options.Serializer = new CustomSerializer();
});
```

### gRPC 客户端调用

```csharp
var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = channel.CreateGrpcService<IUserAppService>();
var user = await client.GetUserAsync(1);
```

---

## ⚠️ 注意事项

1. **接口定义** - 服务必须实现 IApplicationService 接口
2. **返回类型** - 方法返回类型必须是可序列化的
3. **HTTP/2** - gRPC 需要 HTTP/2 支持

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
