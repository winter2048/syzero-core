using Autofac;
using SyZero.Logger;

namespace SyZero.Log4Net
{
    public class Log4NetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}
