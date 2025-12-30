# SyZero.AutoMapper

基于 AutoMapper 的对象映射组件，提供简洁的对象转换功能。

## 📦 安装

```bash
dotnet add package SyZero.AutoMapper
```

## ✨ 特性

- 🚀 **自动配置** - 自动扫描程序集中的映射配置
- 🎯 **简洁 API** - 通过 `IObjectMapper` 接口进行对象转换
- ⚡ **高性能** - 基于 AutoMapper，编译时生成映射代码
- 🔧 **灵活配置** - 支持自定义映射规则

---

## 🚀 快速开始

### 1. 注册服务

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 添加 AutoMapper 服务
builder.Services.AddSyZeroAutoMapper();

var app = builder.Build();

app.Run();
```

### 2. 定义映射配置

创建一个继承自 `Profile` 的类来定义映射规则：

```csharp
using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // 简单映射
        CreateMap<User, UserDto>();
        
        // 反向映射
        CreateMap<User, UserDto>().ReverseMap();
        
        // 自定义映射
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, 
                       opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
```

### 3. 使用 IObjectMapper

```csharp
public class UserService
{
    private readonly IObjectMapper _objectMapper;

    public UserService(IObjectMapper objectMapper)
    {
        _objectMapper = objectMapper;
    }

    public UserDto GetUserDto(User user)
    {
        // 对象转换
        return _objectMapper.Map<UserDto>(user);
    }

    public void UpdateUser(User user, UserDto dto)
    {
        // 更新已有对象
        _objectMapper.Map(dto, user);
    }
}
```

---

## 📖 API 说明

### IObjectMapper 接口

| 方法 | 说明 |
|------|------|
| `Map<TDestination>(object source)` | 将源对象映射为目标类型 |
| `Map<TSource, TDestination>(TSource source, TDestination destination)` | 将源对象映射到已有的目标对象 |

### 使用示例

```csharp
// 单个对象映射
var userDto = _objectMapper.Map<UserDto>(user);

// 集合映射
var userDtos = _objectMapper.Map<List<UserDto>>(users);

// 更新已有对象
_objectMapper.Map(sourceDto, existingEntity);
```

---

## 🔧 高级配置

### 自定义映射规则

```csharp
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            // 忽略某个属性
            .ForMember(dest => dest.InternalId, opt => opt.Ignore())
            
            // 条件映射
            .ForMember(dest => dest.Status, opt => opt.Condition(src => src.IsActive))
            
            // 值转换
            .ForMember(dest => dest.TotalPrice, 
                       opt => opt.MapFrom(src => src.Items.Sum(i => i.Price)))
            
            // 空值处理
            .ForMember(dest => dest.Description, 
                       opt => opt.NullSubstitute("暂无描述"));
    }
}
```

### 嵌套对象映射

```csharp
public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<Address, AddressDto>();
        
        // 嵌套对象会自动映射
        // Customer.Address -> CustomerDto.Address
    }
}
```

### 集合映射

```csharp
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        
        // 集合会自动映射
        // List<Product> -> List<ProductDto>
    }
}
```

---

## 📁 项目结构

```
SyZero.AutoMapper/
├── ObjectMapper.cs              # IObjectMapper 实现
└── SyZeroAutoMapperExtension.cs # 依赖注入扩展方法
```

---

## 🔗 与其他组件集成

### 在应用服务中使用

```csharp
public class ProductAppService : IProductAppService
{
    private readonly IObjectMapper _objectMapper;
    private readonly IRepository<Product> _repository;

    public ProductAppService(
        IObjectMapper objectMapper,
        IRepository<Product> repository)
    {
        _objectMapper = objectMapper;
        _repository = repository;
    }

    public async Task<ProductDto> GetAsync(long id)
    {
        var product = await _repository.GetAsync(id);
        return _objectMapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto input)
    {
        var product = _objectMapper.Map<Product>(input);
        await _repository.InsertAsync(product);
        return _objectMapper.Map<ProductDto>(product);
    }
}
```

---

## ⚠️ 注意事项

1. **映射配置类** - 确保映射配置类（Profile）在被扫描的程序集中
2. **循环引用** - 避免在映射配置中产生循环引用
3. **性能优化** - 对于大量数据映射，考虑使用 `ProjectTo` 进行查询优化

---

## 📄 许可证

MIT License - 详见 [LICENSE](../../../LICENSE)
