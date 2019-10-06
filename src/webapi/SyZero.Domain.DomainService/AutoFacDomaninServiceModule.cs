using Autofac;
namespace SyZero.Application
{
    public class AutoFacDomaninServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblys = System.Reflection.Assembly.Load("SyZero.Domain.DomainService");//Service是继承接口的实现方法类库名称
            var baseType = typeof(Domain.DomainService.IDependency);//IDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(assemblys)
                .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
