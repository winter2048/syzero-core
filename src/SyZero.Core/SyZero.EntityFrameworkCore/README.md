# SyZero.EntityFrameworkCore

SyZero 框架的 Entity Framework Core 集成模块。

## 📦 安装

```bash
dotnet add package SyZero.EntityFrameworkCore
```

## ✨ 特性

- 🚀 **仓储实现** - 基于 EF Core 的仓储模式实现
- 💾 **工作单元** - 事务管理和工作单元模式
- 🔒 **多数据库** - 支持 MySQL、SQL Server 等

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=MyDb;User=root;Password=123456;"
  }
}
```

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
// 添加SyZero
builder.AddSyZero();

// 注册服务方式1 - MySQL
builder.Services.AddSyZeroEntityFramework<MyDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
});

// 注册服务方式2 - SQL Server
builder.Services.AddSyZeroEntityFramework<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// 注册服务方式3 - 带仓储注册
builder.Services.AddSyZeroEntityFramework<MyDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
}).AddRepositories();

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class MyDbContext : SyZeroDbContext<MyDbContext>
{
    public DbSet<User> Users { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) 
        : base(options)
    {
    }
}

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
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `ConnectionString` | `string` | `""` | 数据库连接字符串 |

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

### 事务管理

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task CreateOrderAsync(Order order)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // 业务逻辑
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
```

### 自定义仓储

```csharp
public interface IUserRepository : IRepository<User, long>
{
    Task<User> GetByEmailAsync(string email);
}
```

---

## ⚠️ 注意事项

1. **连接字符串** - 确保配置正确的数据库连接字符串
2. **迁移** - 使用 EF Core 迁移管理数据库结构
3. **性能** - 合理使用 Include 避免 N+1 查询问题

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
