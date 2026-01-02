# SyZero.Swagger

SyZero 框架的 Swagger API 文档模块。

## 📦 安装

```bash
dotnet add package SyZero.Swagger
```

## ✨ 特性

- 🚀 **自动文档** - 自动生成 RESTful API 文档
- 🔒 **JWT 支持** - 内置 Bearer Token 认证支持
- 📖 **XML 注释** - 自动加载 XML 文档注释

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Swagger": {
    "Title": "My API",
    "Version": "v1"
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
builder.Services.AddSwagger();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSwagger(options =>
{
    options.Title = "My API";
    options.Version = "v1";
});

// 注册服务方式3 - 添加多版本支持
builder.Services.AddSwagger(options =>
{
    options.Versions = new[] { "v1", "v2" };
});

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
// 使用 Swagger
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
```

### 3. 使用示例

```csharp
/// <summary>
/// 用户控制器
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
    [HttpGet("{id}")]
    public async Task<UserDto> GetAsync(long id)
    {
        // 实现逻辑
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Title` | `string` | `服务名称` | API 文档标题 |
| `Version` | `string` | `"v1"` | API 版本 |
| `EnableAuth` | `bool` | `true` | 启用认证按钮 |

---

## 📖 API 说明

### Swagger 配置

| 特性 | 说明 |
|------|------|
| `[ApiController]` | 标记 API 控制器 |
| `[HttpGet]` / `[HttpPost]` 等 | HTTP 方法标记 |
| XML 注释 | 自动解析为 API 描述 |

> 确保项目启用 XML 文档生成

---

## 🔧 高级用法

### 启用 XML 文档

在项目文件中添加：

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

### 自定义 UI

```csharp
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = "docs";
});
```

---

## ⚠️ 注意事项

1. **XML 文档** - 必须启用 XML 文档生成才能显示注释
2. **生产环境** - 建议生产环境禁用或限制访问
3. **认证配置** - 使用 Authorize 按钮测试需要认证的接口

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
