# SyZero.Feign

基于 Refit 的声明式 HTTP 客户端组件，用于微服务间的远程调用。

## 📦 安装

```bash
dotnet add package SyZero.Feign
```

## ✨ 特性

- 🚀 **声明式调用** - 通过接口定义远程服务调用
- 🔐 **自动认证** - 自动传递 JWT Token 到远程服务
- 🔄 **服务发现** - 与服务注册中心（Consul/Nacos）集成
- 📦 **统一响应处理** - 自动解析标准响应格式
- ⚡ **Fallback 支持** - 支持服务降级处理

---

## 🚀 快速开始

### 1. 定义服务接口

在共享项目中定义服务接口（继承 `IApplicationService`）：

```csharp
// IUserAppService.cs（共享项目）
public interface IUserAppService : IApplicationService
{
    [Get("/GetUser")]
    Task<UserDto> GetUserAsync(long id);

    [Post("/CreateUser")]
    Task<UserDto> CreateUserAsync([Body] CreateUserDto input);

    [Put("/UpdateUser")]
    Task<UserDto> UpdateUserAsync(long id, [Body] UpdateUserDto input);

    [Delete("/DeleteUser")]
    Task DeleteUserAsync(long id);
}
```

### 2. 实现 Fallback

为接口实现 Fallback 类（服务降级）：

```csharp
// UserAppServiceFallback.cs
public class UserAppServiceFallback : IUserAppService, IFallback
{
    public Task<UserDto> GetUserAsync(long id)
    {
        // 降级处理：返回默认值或抛出异常
        throw new Exception("用户服务暂不可用");
    }

    public Task<UserDto> CreateUserAsync(CreateUserDto input)
    {
        throw new Exception("用户服务暂不可用");
    }

    // ... 其他方法
}
```

### 3. 配置 Feign

在 `appsettings.json` 中添加 Feign 配置：

```json
{
  "Feign": {
    "Service": [
      {
        "ServiceName": "UserService",
        "DllName": "MyApp.Application.Contracts"
      },
      {
        "ServiceName": "OrderService",
        "DllName": "MyApp.Order.Contracts"
      }
    ],
    "Global": {
      "Strategy": "RoundRobin",
      "Retry": 3
    }
  }
}
```

配置说明：
| 字段 | 说明 |
|------|------|
| `ServiceName` | 服务注册中心中的服务名称 |
| `DllName` | 包含服务接口的程序集名称 |
| `Strategy` | 负载均衡策略 |
| `Retry` | 重试次数 |

### 4. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 Feign 服务
builder.Services.AddSyZeroFeign();

var app = builder.Build();

app.Run();
```

### 5. 使用远程服务

```csharp
public class OrderService
{
    private readonly IUserAppService _userService;

    public OrderService(IUserAppService userService)
    {
        _userService = userService;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto input)
    {
        // 调用远程用户服务
        var user = await _userService.GetUserAsync(input.UserId);
        
        if (user == null)
        {
            throw new Exception("用户不存在");
        }

        // 创建订单逻辑...
    }
}
```

---

## 📖 Refit 特性说明

### HTTP 方法

```csharp
public interface IProductAppService : IApplicationService
{
    [Get("/products/{id}")]
    Task<ProductDto> GetAsync(long id);

    [Get("/products")]
    Task<List<ProductDto>> GetListAsync([Query] int pageIndex, [Query] int pageSize);

    [Post("/products")]
    Task<ProductDto> CreateAsync([Body] CreateProductDto input);

    [Put("/products/{id}")]
    Task<ProductDto> UpdateAsync(long id, [Body] UpdateProductDto input);

    [Delete("/products/{id}")]
    Task DeleteAsync(long id);

    [Patch("/products/{id}")]
    Task<ProductDto> PatchAsync(long id, [Body] PatchProductDto input);
}
```

### 参数绑定

```csharp
public interface ISearchAppService : IApplicationService
{
    // 路径参数
    [Get("/items/{category}/{id}")]
    Task<ItemDto> GetItemAsync(string category, long id);

    // 查询参数
    [Get("/search")]
    Task<List<ItemDto>> SearchAsync([Query] string keyword, [Query] int page);

    // 请求体
    [Post("/items")]
    Task<ItemDto> CreateAsync([Body] CreateItemDto input);

    // 请求头
    [Get("/items")]
    Task<List<ItemDto>> GetItemsAsync([Header("X-Custom-Header")] string customHeader);

    // 表单数据
    [Post("/upload")]
    Task UploadAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> formData);
}
```

---

## 🔧 高级用法

### 自定义 API 路由

使用 `[Api]` 特性自定义控制器名称：

```csharp
[Api("custom-users")]  // 路由将变为 /api/{ServiceName}/custom-users/...
public interface IUserAppService : IApplicationService
{
    [Get("/info")]
    Task<UserDto> GetInfoAsync();
}
```

### 处理管道

Feign 使用三层处理管道：

1. **RequestFeignHandler** - 处理请求 URL 构建
2. **AuthenticationFeignHandler** - 添加认证头（Bearer Token）
3. **ResponseFeignHandler** - 解析标准响应格式

```
请求 → RequestHandler → AuthenticationHandler → ResponseHandler → 远程服务
```

### 响应格式

Feign 自动解析标准响应格式：

```json
{
  "code": 0,
  "msg": "success",
  "data": { ... }
}
```

- `code = 0` 时自动提取 `data` 字段返回
- `code != 0` 时抛出 `SyMessageException` 异常

---

## 🔗 与其他组件集成

### 与 Consul 配合使用

```csharp
// 注册 Consul 服务发现
builder.Services.AddSyZeroConsul();

// 注册 Feign
builder.Services.AddSyZeroFeign();
```

### 与 Nacos 配合使用

```csharp
// 注册 Nacos 服务发现
builder.Services.AddSyZeroNacos();

// 注册 Feign
builder.Services.AddSyZeroFeign();
```

---

## 📁 项目结构

```
SyZero.Feign/
├── FeignOptions.cs               # Feign 配置选项
├── AuthenticationFeignHandler.cs # 认证处理器（添加 JWT Token）
├── RequestFeignHandler.cs        # 请求处理器（构建 URL）
├── ResponseFeignHandler.cs       # 响应处理器（解析响应）
└── SyZeroFeignExtension.cs       # 依赖注入扩展方法
```

---

## ⚠️ 注意事项

1. **Fallback 必须实现** - 每个远程服务接口必须有对应的 Fallback 实现类
2. **DllName 配置** - 确保 `DllName` 与包含服务接口的程序集名称完全一致
3. **服务发现** - 使用 Feign 前需先注册服务发现组件（Consul 或 Nacos）
4. **Token 传递** - 自动传递当前会话的 JWT Token 到远程服务
5. **接口定义** - 服务接口必须继承 `IApplicationService`

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
