# SyZero

<p align="center">
  <img src="doc/icon/logo.png" alt="SyZero Logo" width="120"/>
</p>

<p align="center">
  <strong>一个轻量级、模块化的 .NET 微服务开发框架</strong>
</p>

<p align="center">
  <a href="https://github.com/winter2048/syzero-core"><img src="https://img.shields.io/github/stars/winter2048/syzero-core?style=flat-square" alt="GitHub Stars"/></a>
  <a href="https://github.com/winter2048/syzero-core/blob/main/LICENSE"><img src="https://img.shields.io/badge/license-Apache%202.0-blue?style=flat-square" alt="License"/></a>
  <a href="https://www.nuget.org/packages/SyZero"><img src="https://img.shields.io/nuget/v/SyZero?style=flat-square" alt="NuGet"/></a>
  <a href="https://syzero.com"><img src="https://img.shields.io/badge/docs-syzero.com-green?style=flat-square" alt="Documentation"/></a>
</p>

---

## ✨ 简介

SyZero 是一个基于 .NET 的模块化微服务开发框架，提供了丰富的组件和工具，帮助开发者快速构建高性能、可扩展的分布式应用程序。

## 🚀 特性

- 🎯 **模块化设计** - 按需引用，灵活组合
- 🔌 **即插即用** - 简洁的扩展方法，快速集成
- 📦 **丰富的组件** - 涵盖 ORM、缓存、消息队列、服务注册等
- 🌐 **微服务支持** - 内置服务注册发现、API 网关、gRPC 等
- 📊 **可观测性** - 集成 OpenTelemetry 链路追踪
- 🔧 **依赖注入** - 基于 Microsoft.Extensions.DependencyInjection
- 🏥 **健康检查** - 内置服务健康检查与自动清理机制
- 🗳️ **Leader 选举** - 支持多实例部署的 Leader 选举机制

## 📦 核心模块

| 模块 | NuGet 包 | 说明 |
|------|----------|------|
| **SyZero** | [![NuGet](https://img.shields.io/nuget/v/SyZero?style=flat-square)](https://www.nuget.org/packages/SyZero) | 核心模块，提供基础功能和依赖注入 |
| **SyZero.AspNetCore** | [![NuGet](https://img.shields.io/nuget/v/SyZero.AspNetCore?style=flat-square)](https://www.nuget.org/packages/SyZero.AspNetCore) | ASP.NET Core 集成 |
| **SyZero.DynamicWebApi** | [![NuGet](https://img.shields.io/nuget/v/SyZero.DynamicWebApi?style=flat-square)](https://www.nuget.org/packages/SyZero.DynamicWebApi) | 动态 Web API 生成 |
| **SyZero.DynamicGrpc** | [![NuGet](https://img.shields.io/nuget/v/SyZero.DynamicGrpc?style=flat-square)](https://www.nuget.org/packages/SyZero.DynamicGrpc) | 动态 gRPC 服务生成 |
| **SyZero.Swagger** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Swagger?style=flat-square)](https://www.nuget.org/packages/SyZero.Swagger) | Swagger API 文档 |

### 数据访问

| 模块 | NuGet 包 | 说明 |
|------|----------|------|
| **SyZero.EntityFrameworkCore** | [![NuGet](https://img.shields.io/nuget/v/SyZero.EntityFrameworkCore?style=flat-square)](https://www.nuget.org/packages/SyZero.EntityFrameworkCore) | Entity Framework Core 集成 (SQL Server/MySQL) |
| **SyZero.SqlSugar** | [![NuGet](https://img.shields.io/nuget/v/SyZero.SqlSugar?style=flat-square)](https://www.nuget.org/packages/SyZero.SqlSugar) | SqlSugar ORM 集成 |
| **SyZero.MongoDB** | [![NuGet](https://img.shields.io/nuget/v/SyZero.MongoDB?style=flat-square)](https://www.nuget.org/packages/SyZero.MongoDB) | MongoDB 数据库支持 |

### 缓存与消息

| 模块 | NuGet 包 | 说明 |
|------|----------|------|
| **SyZero.Redis** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Redis?style=flat-square)](https://www.nuget.org/packages/SyZero.Redis) | Redis 缓存支持 |
| **SyZero.RabbitMQ** | [![NuGet](https://img.shields.io/nuget/v/SyZero.RabbitMQ?style=flat-square)](https://www.nuget.org/packages/SyZero.RabbitMQ) | RabbitMQ 消息队列 |

### 服务治理

| 模块 | NuGet 包 | 说明 |
|------|----------|------|
| **SyZero.Consul** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Consul?style=flat-square)](https://www.nuget.org/packages/SyZero.Consul) | Consul 服务注册与发现 |
| **SyZero.Nacos** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Nacos?style=flat-square)](https://www.nuget.org/packages/SyZero.Nacos) | Nacos 服务注册与配置中心 |
| **SyZero.ApiGateway** | [![NuGet](https://img.shields.io/nuget/v/SyZero.ApiGateway?style=flat-square)](https://www.nuget.org/packages/SyZero.ApiGateway) | API 网关支持 |
| **SyZero.Feign** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Feign?style=flat-square)](https://www.nuget.org/packages/SyZero.Feign) | 声明式 HTTP 客户端 |

> 💡 **内置服务管理**：SyZero 核心模块还提供了 `LocalServiceManagement`（基于文件）和 `DBServiceManagement`（基于数据库）两种轻量级服务管理实现，适用于开发测试或简单部署场景。

### 工具与扩展

| 模块 | NuGet 包 | 说明 |
|------|----------|------|
| **SyZero.AutoMapper** | [![NuGet](https://img.shields.io/nuget/v/SyZero.AutoMapper?style=flat-square)](https://www.nuget.org/packages/SyZero.AutoMapper) | AutoMapper 对象映射 |
| **SyZero.Log4Net** | [![NuGet](https://img.shields.io/nuget/v/SyZero.Log4Net?style=flat-square)](https://www.nuget.org/packages/SyZero.Log4Net) | Log4Net 日志支持 |
| **SyZero.OpenTelemetry** | [![NuGet](https://img.shields.io/nuget/v/SyZero.OpenTelemetry?style=flat-square)](https://www.nuget.org/packages/SyZero.OpenTelemetry) | OpenTelemetry 分布式追踪 |

## 🛠️ 快速开始

### 安装

通过 NuGet 安装核心包：

```bash
dotnet add package SyZero
```

根据需要安装其他模块：

```bash
dotnet add package SyZero.AspNetCore
dotnet add package SyZero.DynamicWebApi
dotnet add package SyZero.SqlSugar
dotnet add package SyZero.Swagger
```

### 基础使用

```csharp
using SyZero;
using SyZero.DynamicWebApi;

var builder = WebApplication.CreateBuilder(args);

// 使用 SyZero
builder.AddSyZero();

// 动态 WebApi
builder.Services.AddDynamicWebApi(new DynamicWebApiOptions()
{
    DefaultApiPrefix = "/api",
    DefaultAreaName = "MyService"
});

// Swagger 文档
builder.Services.AddSwagger();

// SqlSugar ORM
builder.Services.AddSyZeroSqlSugar<MyDbContext>();

// AutoMapper
builder.Services.AddSyZeroAutoMapper();

var app = builder.Build();

app.UseSyZero();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
```

### 配置文件示例

`appsettings.json`:

```json
{
  "SyZero": {
    "Name": "MyService",
    "Protocol": "http",
    "Port": 5000,
    "Ip": "",
    "WanIp": ""
  },
  "ConnectionString": {
    "DbType": "MySql",
    "ConnectionString": "Server=localhost;Database=mydb;User=root;Password=123456;"
  }
}
```

### 依赖注入

SyZero 支持通过接口自动注入：

```csharp
// Scoped 生命周期
public class MyService : IScopedDependency
{
    // ...
}

// Singleton 生命周期
public class MySingletonService : ISingletonDependency
{
    // ...
}

// Transient 生命周期
public class MyTransientService : ITransientDependency
{
    // ...
}
```

## 🏥 服务管理

SyZero 提供了统一的 `IServiceManagement` 接口，支持多种服务注册发现后端：

| 实现 | 适用场景 | 特点 |
|------|----------|------|
| **LocalServiceManagement** | 开发测试、单机部署 | 基于本地文件，无需外部依赖 |
| **DBServiceManagement** | 简单生产环境 | 基于数据库，支持多实例 |
| **ConsulServiceManagement** | 生产环境 | 基于 Consul，功能完整 |
| **NacosServiceManagement** | 生产环境 | 基于 Nacos，支持配置中心 |

### 核心功能

- **服务注册/注销** - 自动注册服务实例，应用关闭时自动注销
- **健康检查** - 支持 HTTP 健康端点检查和心跳检测
- **自动清理** - 自动清理过期未心跳的服务实例
- **负载均衡** - 支持加权随机负载均衡
- **Leader 选举** - 多实例部署时，仅 Leader 执行健康检查和清理

### 使用示例

```csharp
// 配置服务管理（使用本地文件）
builder.Services.AddSyZeroLocalServiceManagement(options =>
{
    options.EnableHealthCheck = true;
    options.HealthCheckIntervalSeconds = 10;
    options.AutoCleanExpiredServices = true;
    options.EnableLeaderElection = true;  // 启用 Leader 选举
});

// 或使用 Consul
builder.Services.AddSyZeroConsul();

// 或使用 Nacos  
builder.Services.AddSyZeroNacos();
```

### Leader 选举配置

当多个服务实例同时运行时，启用 Leader 选举可避免并发写入冲突：

```csharp
options.EnableLeaderElection = true;       // 启用 Leader 选举
options.LeaderLockExpireSeconds = 30;      // Leader 锁过期时间
options.LeaderLockRenewIntervalSeconds = 10; // Leader 锁续期间隔
```

## 📁 项目结构

```
syzero-core/
├── src/
│   ├── SyZero.Core/                    # 核心模块
│   │   ├── SyZero/                     # 核心库
│   │   ├── SyZero.AspNetCore/          # ASP.NET Core 集成
│   │   ├── SyZero.AutoMapper/          # AutoMapper 支持
│   │   ├── SyZero.Consul/              # Consul 服务发现
│   │   ├── SyZero.DynamicGrpc/         # 动态 gRPC
│   │   ├── SyZero.DynamicWebApi/       # 动态 WebApi
│   │   ├── SyZero.EntityFrameworkCore/ # EF Core 支持
│   │   ├── SyZero.Feign/               # 声明式 HTTP 客户端
│   │   ├── SyZero.Log4Net/             # Log4Net 日志
│   │   ├── SyZero.MongoDB/             # MongoDB 支持
│   │   ├── SyZero.Nacos/               # Nacos 支持
│   │   ├── SyZero.OpenTelemetry/       # 链路追踪
│   │   ├── SyZero.RabbitMQ/            # RabbitMQ 消息队列
│   │   ├── SyZero.Redis/               # Redis 缓存
│   │   ├── SyZero.SqlSugar/            # SqlSugar ORM
│   │   ├── SyZero.Swagger/             # Swagger 文档
│   │   └── SyZero.Web.Common/          # Web 公共组件
│   ├── SyZero.Gateway/                 # API 网关示例
│   └── SyZero.Service/                 # 示例服务
│       ├── SyZero.Example1.Service/    # 示例服务 1
│       └── SyZero.Example2.Service/    # 示例服务 2
├── doc/                                # 文档
├── nuget/                              # NuGet 发布脚本
└── README.md
```

## 🔧 开发环境

- **.NET SDK**: 9.0+
- **IDE**: Visual Studio 2022 / VS Code / Rider
- **数据库**: SQL Server / MySQL / MongoDB (可选)
- **缓存**: Redis (可选)
- **消息队列**: RabbitMQ (可选)
- **服务注册**: Consul / Nacos / 内置 Local/DB (可选)

## 📖 文档

访问 [syzero.com](https://syzero.com) 获取完整文档。

## 📋 更新历史

查看完整的 [更新日志](ReleaseNotes.md)。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

## 📄 许可证

本项目基于 [Apache License 2.0](LICENSE) 许可证开源。

## 👤 作者

**winter2048**

- GitHub: [@winter2048](https://github.com/winter2048)

## ⭐ Star History

如果这个项目对你有帮助，请给一个 Star ⭐

---

<p align="center">Made with ❤️ by winter2048</p>
