# SyZero.AspNetCore

SyZero 框架的 ASP.NET Core Web 层扩展，提供统一的 Web 应用配置和中间件支持。

## 📦 安装

```bash
dotnet add package SyZero.AspNetCore
```

## ✨ 特性

- 🚀 **统一配置** - 一键配置 ASP.NET Core 应用
- 🎯 **MVC 扩展** - 自定义 MVC 选项和过滤器
- 🔒 **异常处理** - 统一的异常处理中间件
- 📝 **日志集成** - 集成 SyZero 日志系统

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Server": {
    "Name": "MyWebApp",
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

// 注册服务方式1 - 使用默认配置
builder.Services.AddSyZeroAspNet();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroAspNet(options =>
{
    options.EnableExceptionHandler = true;
    options.EnableModelValidation = true;
});

// 注册服务方式3 - 添加 MVC 扩展
builder.Services.AddControllers()
    .AddSyZeroMvcOptions();

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : SyZeroControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<UserDto> GetAsync(long id)
    {
        return await _userService.GetUserAsync(id);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `EnableExceptionHandler` | `bool` | `true` | 启用统一异常处理 |
| `EnableModelValidation` | `bool` | `true` | 启用模型验证 |
| `EnableCors` | `bool` | `false` | 启用跨域支持 |

---

## 📖 API 说明

### SyZeroControllerBase 基类

| 属性/方法 | 说明 |
|------|------|
| `CurrentUser` | 获取当前登录用户 |
| `Success(data)` | 返回成功响应 |
| `Fail(message)` | 返回失败响应 |

> 所有控制器推荐继承自 `SyZeroControllerBase`

---

## 🔧 高级用法

### 自定义异常处理

```csharp
builder.Services.AddSyZeroAspNet(options =>
{
    options.ExceptionHandler = (context, exception) =>
    {
        // 自定义异常处理逻辑
    };
});
```

### 添加自定义过滤器

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomActionFilter>();
}).AddSyZeroMvcOptions();
```

---

## ⚠️ 注意事项

1. **中间件顺序** - `UseSyZero()` 应在其他中间件之前调用
2. **异常处理** - 统一异常处理会捕获所有未处理异常
3. **模型验证** - 启用后会自动返回验证错误响应

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
