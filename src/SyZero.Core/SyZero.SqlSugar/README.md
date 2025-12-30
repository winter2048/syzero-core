# SyZero.SqlSugar

基于 SqlSugar ORM 的数据访问组件，提供仓储模式和工作单元支持。

## 📦 安装

```bash
dotnet add package SyZero.SqlSugar
```

## ✨ 特性

- 🚀 **多数据库支持** - 支持 MySQL、SqlServer、PostgreSQL、Oracle、SQLite 等
- 🔄 **读写分离** - 支持主从数据库配置
- 📦 **仓储模式** - 内置泛型仓储，开箱即用
- 🔒 **工作单元** - 支持事务管理
- 🏷️ **属性映射** - 支持标准 DataAnnotations 属性
- ⚡ **自动建表** - 支持 CodeFirst 自动初始化表结构

---

## 🚀 快速开始

### 1. 配置连接字符串

在 `appsettings.json` 中添加数据库配置：

```json
{
  "ConnectionOptions": {
    "Type": 0,
    "Master": "Server=localhost;Database=MyDb;User=root;Password=123456;",
    "Slave": [
      {
        "HitRate": 10,
        "ConnectionString": "Server=slave1;Database=MyDb;User=root;Password=123456;"
      }
    ]
  }
}
```

数据库类型 `Type` 对应值：
| 值 | 数据库 |
|---|--------|
| 0 | MySql |
| 1 | SqlServer |
| 2 | SQLite |
| 3 | Oracle |
| 4 | PostgreSQL |

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 方式一：使用默认 DbContext
builder.Services.AddSyZeroSqlSugar();

// 方式二：使用自定义 DbContext
builder.Services.AddSyZeroSqlSugar<MyDbContext>();

var app = builder.Build();

// 可选：自动初始化表结构
app.InitTables();

app.Run();
```

### 3. 定义实体

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SyZero.Domain.Entities;

[Table("users")]
public class User : Entity
{
    [Required]
    [Column("user_name")]
    public string UserName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    public int? Age { get; set; }  // 可空类型自动设为 IsNullable
}
```

### 4. 使用仓储

```csharp
public class UserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserAsync(long id)
    {
        return await _userRepository.GetModelAsync(id);
    }

    public async Task<IQueryable<User>> GetActiveUsersAsync()
    {
        return await _userRepository.GetListAsync(u => u.IsActive);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        return await _userRepository.AddAsync(user);
    }
}
```

---

## 📖 API 说明

### IRepository<TEntity> 接口

#### 查询方法
| 方法 | 说明 |
|------|------|
| `GetModel(long id)` | 根据 ID 获取实体 |
| `GetModel(Expression<Func<TEntity, bool>> where)` | 根据条件获取单个实体 |
| `GetList()` | 获取所有实体 |
| `GetList(Expression<Func<TEntity, bool>> where)` | 根据条件获取实体列表 |
| `GetPaged(...)` | 分页查询 |
| `Count(Expression<Func<TEntity, bool>> where)` | 统计数量 |

#### 新增方法
| 方法 | 说明 |
|------|------|
| `Add(TEntity entity)` | 添加单个实体 |
| `AddList(IQueryable<TEntity> entities)` | 批量添加实体 |

#### 更新方法
| 方法 | 说明 |
|------|------|
| `Update(TEntity entity)` | 更新单个实体 |
| `Update(IQueryable<TEntity> entities)` | 批量更新实体 |

#### 删除方法
| 方法 | 说明 |
|------|------|
| `Delete(long id)` | 根据 ID 删除 |
| `Delete(Expression<Func<TEntity, bool>> where)` | 根据条件删除 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

### IUnitOfWork 接口

| 方法 | 说明 |
|------|------|
| `BeginTransaction()` | 开启事务 |
| `CommitTransaction()` | 提交事务 |
| `RollbackTransaction()` | 回滚事务 |
| `DisposeTransaction()` | 释放事务 |

---

## 🔧 高级用法

### 自定义 DbContext

```csharp
public class MyDbContext : SyZeroDbContext
{
    public MyDbContext(ConnectionConfig config) : base(config)
    {
        // 自定义 AOP 处理
        this.Context.Aop.OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine($"执行 SQL: {sql}");
        };
    }
}
```

### 使用工作单元（事务）

```csharp
public class OrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<OrderItem> orderItemRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateOrderAsync(Order order, List<OrderItem> items)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _orderRepository.AddAsync(order);
            foreach (var item in items)
            {
                item.OrderId = order.Id;
                await _orderItemRepository.AddAsync(item);
            }

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

### 分页查询

```csharp
public async Task<IQueryable<User>> GetUsersPagedAsync(int pageIndex, int pageSize)
{
    // 按创建时间降序分页
    return await _userRepository.GetPagedAsync(
        pageIndex, 
        pageSize, 
        u => u.CreationTime, 
        isDesc: true
    );
}

public async Task<IQueryable<User>> SearchUsersAsync(string keyword, int pageIndex, int pageSize)
{
    // 带条件的分页查询
    return await _userRepository.GetPagedAsync(
        pageIndex, 
        pageSize, 
        u => u.CreationTime,
        u => u.UserName.Contains(keyword),
        isDesc: true
    );
}
```

### 实体属性映射

支持标准的 `System.ComponentModel.DataAnnotations` 属性：

```csharp
[Table("tb_products")]           // 指定表名
public class Product : Entity
{
    [Key]                         // 主键
    public override long Id { get; set; }

    [Required]                    // 非空（string 类型默认可空）
    [Column("product_name")]      // 指定列名
    public string Name { get; set; }

    [NotMapped]                   // 忽略映射
    public string TempData { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
    public int OrderNo { get; set; }
}
```

---

## 📁 项目结构

```
SyZero.SqlSugar/
├── DbContext/
│   ├── ISyZeroDbContext.cs      # DbContext 接口
│   └── SyZeroDbContext.cs       # DbContext 实现
├── Repositories/
│   └── SqlSugarRepository.cs    # 泛型仓储实现
├── UnitOfWork/
│   └── UnitOfWork.cs            # 工作单元实现
└── SyZeroSqlSugarExtension.cs   # 依赖注入扩展方法
```

---

## ⚠️ 注意事项

1. **连接配置** - 确保 `appsettings.json` 中的 `ConnectionOptions` 配置正确
2. **实体基类** - 实体类需要继承 `Entity` 或实现 `IEntity` 接口
3. **表初始化** - `InitTables()` 会自动创建所有继承 `IEntity` 的实体对应的表
4. **读写分离** - 配置 `Slave` 节点后自动启用读写分离，读操作随机分配到从库

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
