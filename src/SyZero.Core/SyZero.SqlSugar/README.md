# SyZero.SqlSugar

SyZero 框架的 SqlSugar ORM 集成模块。

## 📦 安装

```bash
dotnet add package SyZero.SqlSugar
```

## ✨ 特性

- 🚀 **多数据库** - 支持 MySQL、SQL Server、Oracle、PostgreSQL 等
- 💾 **仓储实现** - 基于 SqlSugar 的仓储模式实现
- 🔒 **事务支持** - 完整的事务管理支持
- ⚡ **高性能** - SqlSugar 高性能 ORM

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "SqlSugar": {
    "ConnectionString": "Server=localhost;Database=MyDb;User=root;Password=123456;",
    "DbType": "MySql"
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
builder.Services.AddSyZeroSqlSugar();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroSqlSugar(options =>
{
    options.ConnectionString = "Server=localhost;Database=MyDb;User=root;Password=123456;";
    options.DbType = DbType.MySql;
});

// 注册服务方式3 - 多数据库
builder.Services.AddSyZeroSqlSugar(options =>
{
    options.Connections = new[]
    {
        new ConnectionConfig { ConfigId = "main", ConnectionString = "...", DbType = DbType.MySql },
        new ConnectionConfig { ConfigId = "log", ConnectionString = "...", DbType = DbType.SqlServer }
    };
});

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class UserService
{
    private readonly IRepository<User, long> _userRepository;

    public UserService(IRepository<User, long> userRepository)
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
| `ConnectionString` | `string` | `""` | 数据库连接字符串 |
| `DbType` | `DbType` | `MySql` | 数据库类型 |
| `IsAutoCloseConnection` | `bool` | `true` | 自动关闭连接 |

---

## 📖 API 说明

### IRepository<TEntity, TPrimaryKey> 接口

| 方法 | 说明 |
|------|------|
| `GetAsync(id)` | 根据主键获取实体 |
| `GetListAsync(predicate)` | 根据条件获取列表 |
| `InsertAsync(entity)` | 插入实体 |
| `UpdateAsync(entity)` | 更新实体 |
| `DeleteAsync(id)` | 删除实体 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 原生 SQL 查询

```csharp
var users = await _db.Ado.SqlQueryAsync<User>(
    "SELECT * FROM Users WHERE Status = @Status",
    new { Status = 1 }
);
```

### 分表分库

```csharp
builder.Services.AddSyZeroSqlSugar(options =>
{
    options.ConfigureExternalServices = new ConfigureExternalServices
    {
        SplitTableService = new SplitTableService()
    };
});
```

---

## ⚠️ 注意事项

1. **连接字符串** - 确保配置正确的数据库连接字符串
2. **数据库类型** - DbType 必须与实际数据库匹配
3. **性能** - 大数据量操作建议使用批量方法

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
