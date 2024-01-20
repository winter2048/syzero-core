using Microsoft.Extensions.DependencyInjection;
using SyZero.Logger;

namespace SyZero
{
    public static class SyZeroLog4NetExtension
    {

        /// <summary>
        /// 注册Log4NetModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroLog4Net(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, Log4Net.Logger>();
            return services;
        }
    }
}
