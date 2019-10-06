using Autofac;
using Autofac.Extensions.DependencyInjection;
using SyZero.ObjectMapper;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SyZero.AutoMapper
{
    /// <summary>
    /// 实体映射注入
    /// </summary>
    public class AutoMapperModule : Module
    {
        private IServiceCollection _services;
        public AutoMapperModule(IServiceCollection services)
        {
            _services = services;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Populate(_services.AddAutoMapper());
            builder.RegisterType<ObjectMapper>().As<SyZero.ObjectMapper.IObjectMapper>();
        }
    }
}
