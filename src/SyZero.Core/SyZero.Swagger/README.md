# SyZero.Swagger

基于 Swashbuckle.AspNetCore 的 Swagger 文档组件，提供 API 文档自动生成和 JWT 认证支持。

## 📦 安装

```bash
dotnet add package SyZero.Swagger
```

## ✨ 特性

- 🚀 **自动配置** - 一行代码启用 Swagger 文档
- 🔐 **JWT 认证** - 内置 Bearer Token 认证支持
- 📝 **XML 注释** - 自动加载所有 XML 文档注释
- 🔄 **接口注释** - 支持从接口定义读取 XML 注释
- 📋 **服务名称** - 自动使用服务名称作为文档标题

---

## 🚀 快速开始

### 1. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 Swagger 服务
builder.Services.AddSwagger();

var app = builder.Build();

// 启用 Swagger 中间件
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.Run();
```

### 2. 配置服务名称

在 `appsettings.json` 中配置服务名称：

```json
{
  "ServerOptions": {
    "Name": "MyService"
  }
}
```

文档标题将自动显示为 "MyService接口文档"。

### 3. 添加 XML 注释

确保项目启用 XML 文档生成：

```xml
<!-- .csproj -->
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

在控制器和方法上添加注释：

```csharp
/// <summary>
/// 用户管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户信息</returns>
    /// <response code="200">返回用户信息</response>
    /// <response code="404">用户不存在</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(long id)
    {
        // ...
    }
}
```

---

## 📖 功能说明

### JWT 认证

Swagger 文档自动配置了 Bearer Token 认证：

1. 点击页面右上角的 **Authorize** 按钮
2. 输入 JWT Token（不需要添加 "Bearer " 前缀）
3. 点击 **Authorize** 确认

之后所有请求都会自动带上 `Authorization: Bearer <token>` 请求头。

### 接口注释支持

该组件扩展了 Swashbuckle 的 XML 注释处理，支持从接口定义中读取注释。这对于使用 DynamicWebApi 等动态生成控制器的场景特别有用：

```csharp
/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserAppService
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input">创建用户请求</param>
    /// <returns>用户信息</returns>
    Task<UserDto> CreateAsync(CreateUserDto input);
}

// 实现类即使没有注释，Swagger 也会显示接口上的注释
public class UserAppService : IUserAppService
{
    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        // ...
    }
}
```

### 自动加载 XML 文档

组件会自动扫描应用程序目录下的所有 `*.xml` 文件并加载为文档注释，无需手动配置。

---

## 🔧 高级配置

### 自定义 Swagger 配置

如果需要更多自定义配置，可以在 `AddSwagger()` 之后继续配置：

```csharp
builder.Services.AddSwagger();

// 追加自定义配置
builder.Services.Configure<SwaggerGenOptions>(options =>
{
    // 添加自定义过滤器
    options.OperationFilter<MyCustomFilter>();
    
    // 添加其他文档
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "API V2"
    });
});
```

### 配置 SwaggerUI

```csharp
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    
    // 文档展开设置
    c.DocExpansion(DocExpansion.None);
    
    // 默认模型展开深度
    c.DefaultModelsExpandDepth(2);
    
    // 显示请求持续时间
    c.DisplayRequestDuration();
});
```

---

## 📁 项目结构

```
SyZero.Swagger/
├── SwaggerExtensions.cs            # Swagger 扩展方法
├── XmlCommentsOperation2Filter.cs  # XML 注释操作过滤器
└── XmlCommentsMemberNameHelper.cs  # XML 注释成员名称助手
```

---

## 🔗 与其他组件集成

### 与 DynamicWebApi 配合使用

```csharp
// 服务注册
builder.Services.AddDynamicWebApi();
builder.Services.AddSwagger();

// 中间件配置
app.UseSwagger();
app.UseSwaggerUI();
```

接口服务上的 XML 注释会自动显示在 Swagger 文档中。

### 与 AspNetCore 配合使用

```csharp
builder.Services.AddSyZeroAspNet();
builder.Services.AddSwagger();
```

---

## ⚠️ 注意事项

1. **XML 文档** - 确保生成 XML 文档文件并与 DLL 放在同一目录
2. **接口注释** - 使用 DynamicWebApi 时，注释应写在接口上而不是实现类
3. **Token 格式** - 在 Swagger UI 中输入 Token 时不需要添加 "Bearer " 前缀

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
