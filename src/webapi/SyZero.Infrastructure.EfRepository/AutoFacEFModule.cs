using Microsoft.EntityFrameworkCore;
using Autofac;
using SyZero.Domain.Repository;
using SyZero.EntityFrameworkCore.Repository;

namespace SyZero.EntityFrameworkCore
{
    public class AutoFacEFModule : Module
    {
        private string _sqlConnection;
        public AutoFacEFModule(string sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        protected override void Load(ContainerBuilder builder)
        {
            // 首先注册 options，供 DbContext 服务初始化使用
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<SyDbContext>();
                //optionsBuilder.UseSqlServer(_sqlConnection);
                //  optionsBuilder.UseMySql(_sqlConnection);
                optionsBuilder.UseMySql(_sqlConnection);
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
