using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SyZero
{
    public static class SyZeroExtension
    {

        public static AutofacServiceProvider AddSyZeroAutofac(this IServiceCollection service, Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();//实例化 AutoFac  容器            
            builder.Populate(service);

            action.Invoke(builder);

            IContainer ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }
    }
}
