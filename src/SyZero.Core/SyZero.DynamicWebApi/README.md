# SyZero.DynamicWebApi

动态 WebApi 生成框架，无需手动创建 Controller，自动从服务接口生成 RESTful API。

## 📦 安装

```bash
dotnet add package SyZero.DynamicWebApi
```

## ✨ 特性

- 🚀 **自动控制器生成** - 从 `IDynamicApi` 接口自动生成 WebApi 控制器
- 🎯 **零配置** - 只需标记 `[DynamicApi]` 即可生成 RESTful API
- ⚡ **高性能缓存** - 使用 `ConcurrentDictionary` 缓存反射结果
- 🔧 **灵活配置** - 支持自定义路由前缀、HTTP 动词映射等
- 📖 **Swagger 集成** - 自动生成 API 文档

---

## 🚀 快速开始

### 1. 定义数据模型

```csharp
public class UserRequest
{
    public long Id { get; set; }
}

public class UserResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 2. 定义服务接口

```csharp
using SyZero.Application.Service;
using SyZero.Application.Attributes;

[DynamicApi]  // 标记在接口层，自动生成 WebApi
public interface IUserService : IApplicationService, IDynamicApi
{
    Task<UserResponse> GetUser(UserRequest request);
    
    Task<UserResponse> CreateUser(CreateUserRequest request);
    
    Task<UserResponse> UpdateUser(UpdateUserRequest request);
    
    Task DeleteUser(UserRequest request);
}
```

### 3. 实现服务

```csharp
public class UserService : IUserService
{
    public Task<UserResponse> GetUser(UserRequest request)
    {
        return Task.FromResult(new UserResponse 
        { 
            Id = request.Id, 
            Name = "John Doe" 
        });
    }

    public Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        // 实现创建逻辑
    }

    public Task<UserResponse> UpdateUser(UpdateUserRequest request)
    {
        // 实现更新逻辑
    }

    public Task DeleteUser(UserRequest request)
    {
        // 实现删除逻辑
    }
}
```

### 4. 配置服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 添加 Dynamic WebApi
builder.Services.AddDynamicWebApi(options =>
{
    options.DefaultApiPrefix = "api";
    options.DefaultAreaName = "v1";
    options.EnableLowerCaseRoutes = true;
});

var app = builder.Build();

app.MapControllers();

app.Run();
```

### 5. 自动生成的 API

上述配置会自动生成以下 API 端点：

| HTTP 方法 | 路由 | 说明 |
|-----------|------|------|
| GET | `/api/v1/user/get` | 获取用户 |
| POST | `/api/v1/user/create` | 创建用户 |
| PUT | `/api/v1/user/update` | 更新用户 |
| DELETE | `/api/v1/user/delete` | 删除用户 |

---

## 📖 配置选项

### DynamicWebApiOptions

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `DefaultApiPrefix` | `string` | `"api"` | API 路由前缀 |
| `DefaultAreaName` | `string` | `null` | 默认区域名称 |
| `EnableLowerCaseRoutes` | `bool` | `false` | 启用小写路由 |
| `RemoveControllerPostfixes` | `List<string>` | `["AppService", ...]` | 移除控制器后缀 |
| `RemoveActionPostfixes` | `List<string>` | `["Async"]` | 移除 Action 后缀 |
| `HttpVerbMappings` | `Dictionary<string, string>` | 默认映射 | HTTP 动词映射 |

### HTTP 动词自动映射

方法名前缀会自动映射到对应的 HTTP 动词：

| 方法前缀 | HTTP 动词 |
|----------|-----------|
| `Get`, `Query`, `Find`, `Fetch`, `Select` | GET |
| `Post`, `Create`, `Add`, `Insert` | POST |
| `Put`, `Update`, `Modify`, `Edit` | PUT |
| `Delete`, `Remove` | DELETE |
| `Patch` | PATCH |

### 配置示例

```csharp
builder.Services.AddDynamicWebApi(options =>
{
    options.DefaultApiPrefix = "api";
    options.DefaultAreaName = "v1";
    options.EnableLowerCaseRoutes = true;
    
    // 自定义 HTTP 动词映射
    options.HttpVerbMappings["Save"] = "POST";
    options.HttpVerbMappings["Batch"] = "POST";
});
```

---

## 🏷️ 特性标记

> **说明**：标记了 `[DynamicApi]` 的接口会自动生成 WebApi 控制器。

### NonWebApiServiceAttribute

排除某个 DynamicApi 服务不生成 WebApi：

```csharp
[DynamicApi]
[NonWebApiService]  // 排除此服务不生成 WebApi
public interface IInternalService : IApplicationService, IDynamicApi
{
    // 此服务不会生成 WebApi，但可以生成 gRPC 服务
}
```

### NonWebApiMethodAttribute

排除某个方法不生成 API 端点：

```csharp
[DynamicApi]
public interface IUserService : IApplicationService, IDynamicApi
{
    Task<UserResponse> GetUser(UserRequest request);
    
    [NonWebApiMethod]  // 此方法不会生成 API 端点
    void InternalMethod();
}
```

### NonDynamicApiAttribute

同时排除 WebApi 和 gRPC：

```csharp
[DynamicApi]
[NonDynamicApi]  // 完全排除，不生成任何 API
public interface IPrivateService : IApplicationService, IDynamicApi
{
}
```

### NonDynamicMethodAttribute

排除某个方法不生成任何 API（包括 gRPC）：

```csharp
[DynamicApi]
public interface IUserService : IApplicationService, IDynamicApi
{
    Task<UserResponse> GetUser(UserRequest request);
    
    [NonDynamicMethod]  // 不生成 WebApi 和 gRPC 方法
    void PrivateMethod();
}
```

---

## 🔗 与 DynamicGrpc 集成

同一服务同时支持 HTTP REST 和 gRPC：

```csharp
[DynamicApi]  // 同时生成 HTTP API 和 gRPC 服务
public interface IUserService : IApplicationService, IDynamicApi
{
    Task<UserResponse> GetUser(UserRequest request);
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

### 仅生成 WebApi（排除 gRPC）

```csharp
[DynamicApi]
[NonGrpcService]  // 只生成 WebApi，不生成 gRPC
public interface IWebOnlyService : IApplicationService, IDynamicApi
{
}
```

### 仅生成 gRPC（排除 WebApi）

```csharp
[DynamicApi]
[NonWebApiService]  // 只生成 gRPC，不生成 WebApi
public interface IGrpcOnlyService : IApplicationService, IDynamicApi
{
}
```

---

## 📁 项目结构

```
SyZero.DynamicWebApi/
├── Attributes/
│   └── WebApiAttributes.cs        # WebApi 特性标记
├── Helpers/
│   ├── ReflectionHelper.cs        # 反射帮助类
│   ├── TypeHelper.cs              # 类型帮助类
│   └── ExtensionMethods.cs        # 扩展方法
├── AppConsts.cs                   # 常量定义
├── AssemblyDynamicWebApiOptions.cs # 程序集配置
├── DynamicWebApiControllerFeatureProvider.cs # 控制器特性提供程序
├── DynamicWebApiConvention.cs     # MVC 约定
├── DynamicWebApiOptions.cs        # 配置选项
└── DynamicWebApiServiceExtensions.cs # 服务扩展方法
```

---

## 📚 Swagger 集成

Dynamic WebApi 自动支持 Swagger 文档生成：

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDynamicWebApi();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
```

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
