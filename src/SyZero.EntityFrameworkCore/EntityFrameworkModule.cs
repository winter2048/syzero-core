using Microsoft.EntityFrameworkCore;
using Autofac;
using SyZero.Domain.Repository;
using SyZero.EntityFrameworkCore.Repository;
using Microsoft.Extensions.Configuration;

namespace SyZero.EntityFrameworkCore
{
    public class EntityFrameworkModule : Module
    {
        private IConfigurationSection _configurationSection { get; set; }
        public EntityFrameworkModule()
        {
        }
        protected override void Load(ContainerBuilder builder)
        {
            // 首先注册 options，供 DbContext 服务初始化使用
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<SyDbContext>();
                if (_configurationSection.GetConnectionString("type").ToLower() == "mysql")
                    optionsBuilder.UseMySql(_configurationSection.GetConnectionString("sqlConnection"));
                else
                    optionsBuilder.UseSqlServer(_configurationSection.GetConnectionString("sqlConnection"));
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();
            // 注册 DbContext
            builder.RegisterType<SyDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();//注册仓储泛型
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        }
    }
}
