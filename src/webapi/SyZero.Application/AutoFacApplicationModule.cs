using Autofac;

namespace SyZero.Application
{
    public class AutoFacApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembliesApplication = System.Reflection.Assembly.Load("SyZero.Application");
            var baseTypeApplication = typeof(Application.IDependency);
            builder.RegisterAssemblyTypes(assembliesApplication)
                 .Where(m => baseTypeApplication.IsAssignableFrom(m) && m != baseTypeApplication)
                 .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
