using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SyZero.EntityFrameworkCore
{
    public static class SyZeroEntityFrameworkExtension
    {
        /// <summary>
        /// 注册EntityFramework
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroEntityFramework<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            // 首先注册 options，供 DbContext 服务初始化使用
            services.AddSingleton(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TContext>();
                if (AppConfig.GetSection("type").ToLower() == "mysql")
                    optionsBuilder.UseMySQL(AppConfig.GetSection("sqlConnection"));
                else
                    optionsBuilder.UseSqlServer(AppConfig.GetSection("sqlConnection"));
                return optionsBuilder.Options;
            });

            services.AddScoped(typeof(TContext));

            return services;
        }
    }
}
