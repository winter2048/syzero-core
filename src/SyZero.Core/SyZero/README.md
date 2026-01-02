# SyZero

SyZero 是一个轻量级的 .NET 微服务框架核心库，提供依赖注入、配置管理、领域驱动等基础功能。

## 📦 安装

```bash
dotnet add package SyZero
```

## ✨ 特性

- 🚀 **依赖注入** - 基于 Microsoft.Extensions.DependencyInjection 的模块化依赖注入
- 💾 **仓储模式** - 通用仓储接口和工作单元模式
- 🔒 **配置管理** - 统一的配置读取和管理
- 🎯 **领域驱动** - 实体、值对象、领域事件等 DDD 基础设施
- 📝 **异常处理** - 统一的业务异常和友好异常处理

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Server": {
    "Name": "MyService",
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

// 注册服务方式1 - 使用配置文件
builder.Services.AddSyZero();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZero(options =>
{
    options.ServerName = "MyService";
    options.ServerPort = 5000;
});

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
// 定义实体
public class User : Entity<long>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// 使用仓储
public class UserService
{
    private readonly IRepository<User, long> _userRepository;

    public UserService(IRepository<User, long> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserAsync(long id)
    {
        return await _userRepository.GetAsync(id);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `ServerName` | `string` | `""` | 服务名称 |
| `ServerPort` | `int` | `5000` | 服务端口 |

---

## 📖 API 说明

### IRepository<TEntity, TPrimaryKey> 接口

| 方法 | 说明 |
|------|------|
| `GetAsync(id)` | 根据主键获取实体 |
| `GetListAsync()` | 获取实体列表 |
| `InsertAsync(entity)` | 插入实体 |
| `UpdateAsync(entity)` | 更新实体 |
| `DeleteAsync(id)` | 删除实体 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 自定义仓储

```csharp
public interface IUserRepository : IRepository<User, long>
{
    Task<User> GetByEmailAsync(string email);
}

public class UserRepository : BaseRepository<User, long>, IUserRepository
{
    public async Task<User> GetByEmailAsync(string email)
    {
        return await GetFirstOrDefaultAsync(u => u.Email == email);
    }
}
```

---

## ⚠️ 注意事项

1. **配置文件** - 确保 appsettings.json 中包含必要的配置节点
2. **依赖注入** - 所有服务都应通过依赖注入获取
3. **异步方法** - 推荐使用异步方法以提高性能

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
