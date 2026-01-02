# SyZero.AutoMapper

SyZero 框架的 AutoMapper 集成模块，提供对象映射自动配置。

## 📦 安装

```bash
dotnet add package SyZero.AutoMapper
```

## ✨ 特性

- 🚀 **自动扫描** - 自动扫描并注册所有 Profile
- 💾 **依赖注入** - 无缝集成 Microsoft DI
- 🔒 **类型安全** - 编译时类型检查

---

## 🚀 快速开始

### 1. 配置 appsettings.json

```json
{
  "AutoMapper": {
    "AssembliesToScan": ["MyApp.Application"]
  }
}
```

### 2. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
// 添加SyZero
builder.AddSyZero();

// 注册服务方式1 - 自动扫描当前程序集
builder.Services.AddSyZeroAutoMapper();

// 注册服务方式2 - 指定程序集
builder.Services.AddSyZeroAutoMapper(typeof(UserProfile).Assembly);

// 注册服务方式3 - 多个程序集
builder.Services.AddSyZeroAutoMapper(
    typeof(UserProfile).Assembly,
    typeof(OrderProfile).Assembly
);

var app = builder.Build();
// 使用SyZero
app.UseSyZero();
app.Run();
```

### 3. 使用示例

```csharp
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserInput, User>();
    }
}

public class UserService
{
    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserDto GetUser(User user)
    {
        return _mapper.Map<UserDto>(user);
    }
}
```

---

## 📖 配置选项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `AssembliesToScan` | `string[]` | `[]` | 要扫描的程序集名称 |

---

## 📖 API 说明

### IMapper 接口

| 方法 | 说明 |
|------|------|
| `Map<TDestination>(source)` | 将源对象映射到目标类型 |
| `Map<TSource, TDestination>(source, dest)` | 映射到现有对象 |
| `Map(source, sourceType, destType)` | 动态类型映射 |

> 所有映射操作都是线程安全的

---

## 🔧 高级用法

### 自定义值转换

```csharp
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, 
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
```

### 条件映射

```csharp
CreateMap<User, UserDto>()
    .ForMember(dest => dest.Email, 
        opt => opt.Condition(src => src.IsEmailVerified));
```

---

## ⚠️ 注意事项

1. **Profile 类** - 所有映射配置应在 Profile 类中定义
2. **循环引用** - 注意处理对象间的循环引用
3. **性能** - 避免在热路径中使用动态映射

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
