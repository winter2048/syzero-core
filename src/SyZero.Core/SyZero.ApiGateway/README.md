# SyZero.ApiGateway

基于 Ocelot 的 API 网关模块，支持服务路由、负载均衡和 Swagger 文档聚合。

## 📦 安装

```bash
dotnet add package SyZero.ApiGateway
```

## ✨ 特性

- 🚀 **服务路由** - 基于 Ocelot 的动态路由配置
- ⚖️ **负载均衡** - 支持多种负载均衡策略
- 📖 **Swagger 聚合** - 使用 MMLib.SwaggerForOcelot 聚合下游服务文档
- 🔄 **熔断降级** - 集成 Polly 实现熔断和重试

---

## 🚀 快速开始

### 1. 配置 ocelot.json

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "localhost", "Port": 5001 }
      ],
      "UpstreamPathTemplate": "/user-service/{everything}",
      "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
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
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();

// 注册服务方式2 - 添加 Polly 熔断
builder.Services.AddOcelot()
    .AddPolly();

// 注册服务方式3 - 添加 Consul 服务发现
builder.Services.AddOcelot()
    .AddConsul();

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.UseOcelot().Wait();
app.Run();
```

### 3. 使用示例

```csharp
// 网关自动转发请求，无需额外代码
// 客户端请求: GET http://gateway:5000/user-service/api/users/1
// 转发到: GET http://user-service:5001/api/users/1
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Routes` | `array` | `[]` | 路由配置数组 |
| `DownstreamPathTemplate` | `string` | `""` | 下游服务路径模板 |
| `UpstreamPathTemplate` | `string` | `""` | 上游请求路径模板 |
| `LoadBalancerOptions.Type` | `string` | `"RoundRobin"` | 负载均衡类型 |

---

## 📖 API 说明

### 路由配置

| 属性 | 说明 |
|------|------|
| `DownstreamHostAndPorts` | 下游服务地址列表 |
| `UpstreamHttpMethod` | 支持的 HTTP 方法 |
| `RateLimitOptions` | 限流配置 |
| `QoSOptions` | 服务质量配置 |

> 详细配置请参考 [Ocelot 文档](https://ocelot.readthedocs.io/)

---

## 🔧 高级用法

### Swagger 文档聚合

```csharp
builder.Services.AddSwaggerForOcelot(builder.Configuration);

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
```

### 自定义中间件

```csharp
builder.Services.AddOcelot()
    .AddDelegatingHandler<CustomHandler>();
```

---

## ⚠️ 注意事项

1. **配置文件** - ocelot.json 必须单独配置，不能合并到 appsettings.json
2. **服务发现** - 使用服务发现时需要配置对应的服务注册中心
3. **熔断配置** - QoS 配置需要与 Polly 配合使用

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
