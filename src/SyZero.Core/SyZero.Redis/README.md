# SyZero.Redis

SyZero 框架的 Redis 缓存和分布式锁模块。

## 📦 安装

```bash
dotnet add package SyZero.Redis
```

## ✨ 特性

- 🚀 **缓存** - 基于 Redis 的分布式缓存
- 🔒 **分布式锁** - 可靠的分布式锁实现
- 💾 **多模式** - 支持主从、哨兵、集群模式
- 🔍 **服务发现** - 可作为服务注册中心

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "Redis": {
    "Type": "MasterSlave",
    "Master": "localhost:6379,password=123456,defaultDatabase=0",
    "Slave": []
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
builder.Services.AddSyZeroRedis();

// 注册服务方式2 - 使用委托配置
builder.Services.AddSyZeroRedis(options =>
{
    options.Type = RedisType.MasterSlave;
    options.Master = "localhost:6379";
});

// 注册服务方式3 - 添加服务发现
builder.Services.AddSyZeroRedis()
    .AddRedisServiceManagement();

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class UserService
{
    private readonly ICache _cache;
    private readonly ILockUtil _lockUtil;

    public UserService(ICache cache, ILockUtil lockUtil)
    {
        _cache = cache;
        _lockUtil = lockUtil;
    }

    public async Task<User> GetUserAsync(long id)
    {
        var cacheKey = $"user:{id}";
        var user = await _cache.GetAsync<User>(cacheKey);
        
        if (user == null)
        {
            user = await LoadUserFromDbAsync(id);
            await _cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(30));
        }
        
        return user;
    }

    public async Task CreateOrderAsync(Order order)
    {
        var lockKey = $"order:create:{order.UserId}";
        
        using (await _lockUtil.LockAsync(lockKey, TimeSpan.FromSeconds(30)))
        {
            // 在锁内执行订单创建逻辑
        }
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Type` | `string` | `"MasterSlave"` | Redis 模式（MasterSlave/Sentinel/Cluster） |
| `Master` | `string` | `""` | 主节点连接字符串 |
| `Slave` | `string[]` | `[]` | 从节点连接字符串列表 |
| `Sentinel` | `string[]` | `[]` | 哨兵节点列表 |

---

## 📖 API 说明

### ICache 接口

| 方法 | 说明 |
|------|------|
| `GetAsync<T>(key)` | 获取缓存值 |
| `SetAsync<T>(key, value, expiration)` | 设置缓存值 |
| `RemoveAsync(key)` | 移除缓存 |
| `ExistsAsync(key)` | 检查缓存是否存在 |

### ILockUtil 接口

| 方法 | 说明 |
|------|------|
| `LockAsync(key, expiration)` | 获取分布式锁 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

---

## 🔧 高级用法

### 哨兵模式

```json
{
  "Redis": {
    "Type": "Sentinel",
    "Master": "mymaster",
    "Sentinel": ["localhost:26379", "localhost:26380"]
  }
}
```

### 集群模式

```json
{
  "Redis": {
    "Type": "Cluster",
    "Master": "localhost:7000",
    "Slave": ["localhost:7001", "localhost:7002"]
  }
}
```

---

## ⚠️ 注意事项

1. **连接字符串** - 确保 Redis 服务可访问
2. **锁超时** - 合理设置锁的超时时间
3. **序列化** - 缓存对象需要可序列化

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
