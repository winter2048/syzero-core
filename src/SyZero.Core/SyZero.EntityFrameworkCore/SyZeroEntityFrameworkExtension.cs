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
            //builder.Register(c =>
            //{
            //    var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            //    if (AppConfig.GetSection("type").ToLower() == "mysql")
            //        optionsBuilder.UseMySql(AppConfig.GetSection("sqlConnection"), new MySqlServerVersion(new Version(8, 0, 23)));
            //    else
            //        optionsBuilder.UseSqlServer(AppConfig.GetSection("sqlConnection"));
            //    return optionsBuilder.Options;
            //}).InstancePerLifetimeScope();


            services.AddScoped(typeof(TContext));

            return services;
        }
    }
}
