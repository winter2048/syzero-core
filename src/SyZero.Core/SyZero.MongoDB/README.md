# SyZero.MongoDB

SyZero 框架的 MongoDB 集成模块。

## 📦 安装

```bash
dotnet add package SyZero.MongoDB
```

## ✨ 特性

- 🚀 **仓储实现** - 基于 MongoDB 的仓储模式实现
- 💾 **文档存储** - 原生文档数据库支持
- 🔒 **查询构建** - 流畅的查询 API

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MyDatabase"
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
builder.Services.AddSyZeroMongoDB();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroMongoDB(options =>
{
    options.ConnectionString = "mongodb://localhost:27017";
    options.DatabaseName = "MyDatabase";
});

// 注册服务方式3 - 指定配置节
builder.Services.AddSyZeroMongoDB(builder.Configuration, "MongoDB");

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class UserService
{
    private readonly IRepository<User, string> _userRepository;

    public UserService(IRepository<User, string> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        return await _userRepository.InsertAsync(user);
    }

    public async Task<List<User>> GetActiveUsersAsync()
    {
        return await _userRepository.GetListAsync(u => u.IsActive);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `ConnectionString` | `string` | `""` | MongoDB 连接字符串 |
| `DatabaseName` | `string` | `""` | 数据库名称 |

---

## 📖 API 说明

### IRepository<TEntity, TPrimaryKey> 接口

| 方法 | 说明 |
|------|------|
| `GetAsync(id)` | 根据主键获取文档 |
| `GetListAsync(filter)` | 根据条件获取文档列表 |
| `InsertAsync(entity)` | 插入文档 |
| `UpdateAsync(entity)` | 更新文档 |
| `DeleteAsync(id)` | 删除文档 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 聚合查询

```csharp
var result = await _userRepository.AggregateAsync(pipeline =>
    pipeline
        .Match(u => u.IsActive)
        .Group(u => u.Department, g => new { Count = g.Count() })
);
```

### 索引管理

```csharp
await _userRepository.CreateIndexAsync(
    Builders<User>.IndexKeys.Ascending(u => u.Email),
    new CreateIndexOptions { Unique = true }
);
```

---

## ⚠️ 注意事项

1. **连接字符串** - 确保 MongoDB 服务可访问
2. **主键类型** - MongoDB 默认使用 ObjectId 作为主键
3. **索引** - 为常用查询字段创建索引以提高性能

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
