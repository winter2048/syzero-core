using Autofac;
using AutoMapper;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace SyZero.AutoMapper
{
    /// <summary>
    /// 实体映射注入
    /// </summary>
    public class AutoMapperModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var asss = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => (lib.Type == "package" && lib.Name.StartsWith("SyZero") || lib.Type == "project"));//排除所有的系统程序集、Nuget下载包
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


            builder.Register(
              c => new MapperConfiguration(cfg =>
              {
                  cfg.AddMaps(asss);
              }))
              .AsSelf()
              .SingleInstance();

            builder.Register(
                c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ObjectMapper>().As<SyZero.ObjectMapper.IObjectMapper>().InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
