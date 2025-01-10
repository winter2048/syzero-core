using log4net.Appender;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using SyZero.Logger;
using System.Linq;

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
        public static IServiceCollection AddSyZeroLog4Net(this IServiceCollection services, string configFile = "log4net.config")
        {
            XmlConfigurator.Configure(new FileInfo(configFile));

#if DEBUG
            var appender = LogManager.GetRepository().GetAppenders().OfType<RollingFileAppender>().FirstOrDefault();
            if (appender != null)
            {
                appender.ImmediateFlush = true; // 确保即时写入
            }
#endif

            services.AddScoped(typeof(ILogger<>), typeof(Log4Net.Logger<>));
            services.AddSingleton<ILogger, Log4Net.Logger<ILogger>>();
            return services;
        }
    }
}
