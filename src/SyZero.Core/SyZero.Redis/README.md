# SyZero.Redis

基于 FreeRedis 的 Redis 缓存组件，提供缓存操作和分布式锁支持。

## 📦 安装

```bash
dotnet add package SyZero.Redis
```

## ✨ 特性

- 🚀 **多种部署模式** - 支持主从、哨兵、集群三种模式
- 💾 **缓存接口** - 实现 `ICache` 接口，提供统一的缓存操作
- 🔒 **分布式锁** - 实现 `ILockUtil` 接口，支持带等待的分布式锁
- 🌐 **服务管理** - 实现 `IServiceManagement` 接口，支持服务注册发现
- ⚡ **高性能** - 基于 FreeRedis，高性能 Redis 客户端
- 🔄 **异步支持** - 所有操作都提供异步版本
- 🏥 **健康检查** - 内置服务健康检查与自动清理机制
- 🗳️ **Leader 选举** - 支持多实例部署的 Leader 选举机制

---

## 🚀 快速开始

### 1. 配置 Redis

在 `appsettings.json` 中添加 Redis 配置：

#### 主从模式

```json
{
  "Redis": {
    "Type": 0,
    "Master": "127.0.0.1:6379,password=123456,defaultDatabase=0",
    "Slave": [
      "127.0.0.1:6380,password=123456,defaultDatabase=0",
      "127.0.0.1:6381,password=123456,defaultDatabase=0"
    ]
  }
}
```

#### 哨兵模式

```json
{
  "Redis": {
    "Type": 1,
    "Master": "mymaster,password=123456,defaultDatabase=0",
    "Sentinel": [
      "127.0.0.1:26379",
      "127.0.0.1:26380",
      "127.0.0.1:26381"
    ]
  }
}
```

#### 集群模式

```json
{
  "Redis": {
    "Type": 2,
    "Master": "127.0.0.1:6379,password=123456",
    "Slave": [
      "127.0.0.1:6380,password=123456",
      "127.0.0.1:6381,password=123456"
    ]
  }
}
```

部署类型 `Type` 对应值：
| 值 | 模式 | 说明 |
|---|------|------|
| 0 | MasterSlave | 主从模式 |
| 1 | Sentinel | 哨兵模式 |
| 2 | Cluster | 集群模式 |

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 Redis 服务
builder.Services.AddSyZeroRedis();

var app = builder.Build();

app.Run();
```

### 3. 使用缓存

```csharp
public class UserService
{
    private readonly ICache _cache;

    public UserService(ICache cache)
    {
        _cache = cache;
    }

    public async Task<User> GetUserAsync(long id)
    {
        var cacheKey = $"user:{id}";
        
        // 从缓存获取
        var user = _cache.Get<User>(cacheKey);
        if (user != null)
        {
            return user;
        }

        // 从数据库获取
        user = await GetUserFromDbAsync(id);
        
        // 写入缓存（过期时间 1 小时）
        await _cache.SetAsync(cacheKey, user, 3600);
        
        return user;
    }
}
```

---

## 📖 API 说明

### ICache 接口

| 方法 | 说明 |
|------|------|
| `Exist(string key)` | 检查键是否存在 |
| `Get<T>(string key)` | 获取缓存值 |
| `GetKeys(string pattern)` | 按模式获取所有键 |
| `Set<T>(string key, T value, int expireTime)` | 设置缓存值（默认过期时间 24 小时） |
| `Remove(string key)` | 删除缓存 |
| `Refresh(string key)` | 刷新缓存 |

> 所有方法都有对应的异步版本（带 `Async` 后缀）

### 使用示例

```csharp
// 设置缓存（过期时间 1 小时 = 3600 秒）
_cache.Set("myKey", myObject, 3600);

// 获取缓存
var obj = _cache.Get<MyClass>("myKey");

// 检查是否存在
if (_cache.Exist("myKey"))
{
    // ...
}

// 删除缓存
_cache.Remove("myKey");

// 批量获取键
var keys = _cache.GetKeys("user:*");
```

---

## 🔒 分布式锁

### ILockUtil 接口

| 方法 | 说明 |
|------|------|
| `GetLock(string lockKey, int expiresSenconds, int waitTimeSenconds)` | 获取锁 |
| `GetLockAsync(...)` | 异步获取锁 |
| `Release(string lockKey)` | 释放锁 |

### 参数说明

| 参数 | 默认值 | 说明 |
|------|--------|------|
| `lockKey` | - | 锁的键名 |
| `expiresSenconds` | 10 | 锁的过期时间（秒），防止死锁 |
| `waitTimeSenconds` | 10 | 等待获取锁的最大时间（秒），0 表示不等待 |

### 使用示例

```csharp
public class OrderService
{
    private readonly ILockUtil _lockUtil;

    public OrderService(ILockUtil lockUtil)
    {
        _lockUtil = lockUtil;
    }

    public async Task<bool> ProcessOrderAsync(long orderId)
    {
        var lockKey = $"order:lock:{orderId}";
        
        // 尝试获取锁（过期时间 30 秒，等待时间 10 秒）
        if (!await _lockUtil.GetLockAsync(lockKey, 30, 10))
        {
            throw new Exception("获取锁失败，请稍后重试");
        }

        try
        {
            // 执行业务逻辑
            await DoBusinessLogicAsync(orderId);
            return true;
        }
        finally
        {
            // 释放锁
            _lockUtil.Release(lockKey);
        }
    }
}
```

### 不等待的锁

```csharp
// waitTimeSenconds = 0 时，立即返回获取结果
if (_lockUtil.GetLock("myLock", expiresSenconds: 10, waitTimeSenconds: 0))
{
    try
    {
        // 获取成功，执行操作
    }
    finally
    {
        _lockUtil.Release("myLock");
    }
}
else
{
    // 获取失败，资源已被占用
}
```

---

## 🔧 高级用法

### 直接使用 RedisClient

如果需要更底层的 Redis 操作，可以直接注入 `RedisClient`：

```csharp
public class MyService
{
    private readonly RedisClient _redis;

    public MyService(RedisClient redis)
    {
        _redis = redis;
    }

    public void DoSomething()
    {
        // 使用 FreeRedis 原生 API
        _redis.HSet("myhash", "field1", "value1");
        _redis.LPush("mylist", "item1");
        _redis.ZAdd("myset", 1, "member1");
        
        // 发布订阅
        _redis.Publish("channel", "message");
    }
}
```

### 缓存模式封装

```csharp
public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int expireSeconds = 3600)
{
    var value = _cache.Get<T>(key);
    if (value != null)
    {
        return value;
    }

    value = await factory();
    if (value != null)
    {
        await _cache.SetAsync(key, value, expireSeconds);
    }

    return value;
}

// 使用
var user = await GetOrSetAsync(
    $"user:{id}", 
    async () => await _userRepository.GetAsync(id),
    3600
);
```

---

## 📁 项目结构

```
SyZero.Redis/
├── Cache.cs                           # ICache 实现
├── LockUtil.cs                        # ILockUtil 实现（分布式锁）
├── RedisOptions.cs                    # Redis 配置选项
├── RedisServiceManagement.cs          # IServiceManagement 实现（服务管理）
├── RedisServiceManagementOptions.cs   # 服务管理配置选项
└── SyZeroRedisExtension.cs            # 依赖注入扩展方法
```

---

## 🌐 服务管理

Redis 服务管理实现了 `IServiceManagement` 接口，适用于分布式部署场景。

### 配置

在 `appsettings.json` 中添加配置：

```json
{
  "Redis": {
    "Type": 0,
    "Master": "127.0.0.1:6379,password=123456,defaultDatabase=0"
  },
  "RedisServiceManagement": {
    "EnableHealthCheck": true,
    "HealthCheckIntervalSeconds": 10,
    "ServiceExpireSeconds": 30,
    "AutoCleanExpiredServices": true,
    "AutoCleanIntervalSeconds": 300,
    "ServiceCleanSeconds": 600,
    "EnableLeaderElection": true,
    "LeaderLockExpireSeconds": 30,
    "LeaderLockRenewIntervalSeconds": 10,
    "EnablePubSub": true
  }
}
```

### 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 Redis 服务管理
builder.Services.AddRedisServiceManagement();

// 或使用自定义配置
builder.Services.AddRedisServiceManagement(options =>
{
    options.EnableHealthCheck = true;
    options.HealthCheckIntervalSeconds = 10;
    options.EnableLeaderElection = true;
});

var app = builder.Build();
app.Run();
```

### 使用示例

```csharp
public class MyService
{
    private readonly IServiceManagement _serviceManagement;

    public MyService(IServiceManagement serviceManagement)
    {
        _serviceManagement = serviceManagement;
    }

    // 注册服务
    public async Task RegisterAsync()
    {
        await _serviceManagement.RegisterService(new ServiceInfo
        {
            ServiceName = "MyService",
            ServiceAddress = "192.168.1.100",
            ServicePort = 5000,
            HealthCheckUrl = "http://192.168.1.100:5000/health"
        });
    }

    // 获取服务实例（负载均衡）
    public async Task<ServiceInfo> GetServiceAsync()
    {
        return await _serviceManagement.GetServiceInstance("MyService");
    }

    // 订阅服务变更
    public async Task SubscribeAsync()
    {
        await _serviceManagement.Subscribe("MyService", services =>
        {
            Console.WriteLine($"服务列表更新，当前实例数: {services.Count}");
        });
    }
}
```

### 配置选项说明

| 配置项 | 默认值 | 说明 |
|--------|--------|------|
| `KeyPrefix` | `syzero:services:` | 服务注册 Key 前缀 |
| `LeaderKeyPrefix` | `syzero:leader:` | Leader 选举 Key 前缀 |
| `EnableHealthCheck` | `true` | 是否启用健康检查 |
| `HealthCheckIntervalSeconds` | `10` | 健康检查间隔（秒） |
| `HealthCheckTimeoutSeconds` | `5` | 健康检查超时时间（秒） |
| `ServiceExpireSeconds` | `30` | 服务过期时间（秒） |
| `AutoCleanExpiredServices` | `true` | 是否自动清理过期服务 |
| `AutoCleanIntervalSeconds` | `300` | 自动清理间隔（秒） |
| `ServiceCleanSeconds` | `600` | 服务清理时间（秒） |
| `EnableLeaderElection` | `true` | 是否启用 Leader 选举 |
| `LeaderLockExpireSeconds` | `30` | Leader 锁过期时间（秒） |
| `LeaderLockRenewIntervalSeconds` | `10` | Leader 锁续期间隔（秒） |
| `EnablePubSub` | `true` | 是否启用发布/订阅通知 |

### Leader 选举

当多个服务实例同时运行时，启用 Leader 选举可避免并发执行健康检查和清理操作：

- **Leader 实例**：负责执行健康检查和过期服务清理
- **非 Leader 实例**：只进行服务注册和查询，不执行健康检查

Leader 选举基于 Redis 的 `SETNX` 命令实现，具有原子性保证。

---

## ⚠️ 注意事项

1. **连接字符串格式** - 使用 FreeRedis 的连接字符串格式：`host:port,password=xxx,defaultDatabase=0`
2. **锁的释放** - 使用分布式锁时，务必在 `finally` 块中释放锁
3. **过期时间** - 合理设置缓存过期时间，避免内存溢出
4. **哨兵模式** - Master 参数填写哨兵配置的主节点名称，而非 IP 地址

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
