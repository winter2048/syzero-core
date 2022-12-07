
using Autofac;
using Autofac.Core;
using Dynamitey;
using ImpromptuInterface;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using SyZero.Application.Service;
using SyZero.Runtime.Session;


namespace SyZero
{
    public class SyZeroModule : Autofac.Module
    {

        public SyZeroModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            System.Console.WriteLine(@" _____  __    __  ______  _____   _____    _____  
/  ___/ \ \  / / |___  / | ____| |  _  \  /  _  \ 
| |___   \ \/ /     / /  | |__   | |_| |  | | | | 
\___  \   \  /     / /   |  __|  |  _  /  | | | | 
 ___| |   / /     / /__  | |___  | | \ \  | |_| | 
/_____/  /_/     /_____| |_____| |_|  \_\ \_____/ ");
            System.Console.WriteLine("");
            System.Console.WriteLine("版本号: " + GetType().Assembly.GetName().Version);
            System.Console.WriteLine("启动中......");

            var asss = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => (lib.Type == "package" && lib.Name.StartsWith("SyZero") || lib.Type == "project"));//排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    System.Console.WriteLine(lib.Name);
                    var assembly = Assembly.Load(new AssemblyName(lib.Name));
                    asss.Add(assembly);
                }
                catch
                {
                }
            }

            //注入SySession
            builder.RegisterType<SySession>().As<ISySession>().InstancePerLifetimeScope().PropertiesAutowired();

            var baseType = typeof(SyZero.Dependency.ITransientDependency);//ITransientDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(asss.ToArray())
                .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

            var baseType2 = typeof(SyZero.Dependency.ISingletonDependency);//ISingletonDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(asss.ToArray())
                .Where(m => baseType2.IsAssignableFrom(m) && m != baseType2)
                .AsImplementedInterfaces().SingleInstance().PropertiesAutowired();


            var applicationServiceType = typeof(SyZero.SyZeroServiceBase);//SyZeroServiceBase 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(asss.ToArray())
                .Where(m => applicationServiceType.IsAssignableFrom(m)).InstancePerLifetimeScope().PropertiesAutowired();
        }




    
    }
}
