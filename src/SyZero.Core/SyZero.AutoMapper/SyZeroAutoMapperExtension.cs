using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SyZero
{
    public static class SyZeroAutoMapperExtension
    {
        /// <summary>
        /// AutoMapperModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(ReflectionHelper.GetAssemblies());
            services.AddScoped<SyZero.ObjectMapper.IObjectMapper, SyZero.AutoMapper.ObjectMapper>();

            return services;
        }
    }
}
