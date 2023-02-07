using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using SyZero.AspNetCore.Controllers;
using SyZero.AspNetCore.Middleware;

namespace SyZero.AspNetCore
{
    public class SyZeroControllerModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SyAuthMiddleware>().InstancePerLifetimeScope();

            var asss = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => (lib.Type == "package" && lib.Name.StartsWith("Dora") || lib.Type == "project"));//排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    //System.Console.WriteLine(lib.Name);
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    asss.Add(assembly);
                }
                catch
                {
                }
            }


            var baseType = typeof(SyZeroController);
            builder.RegisterAssemblyTypes(asss.ToArray())
                .Where(m => baseType.IsAssignableFrom(m) && m != baseType).PropertiesAutowired();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
    }
}
