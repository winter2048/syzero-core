using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace SyZero.Common
{
    /// <summary>
    /// Appsettings配置类
    /// </summary>
    public class UtilConf
    {

        public static readonly IConfiguration Configuration;

        static UtilConf()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();
        }
        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">节点名称</param>
        /// <returns></returns>
        public static T GetSection<T>(string key) where T : class, new()
        {
            var obj = Configuration.GetValue<T>(key);
            return obj;
        }
        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <returns></returns>
        public static string GetSection(string key)
        {
            return Configuration.GetValue<string>(key);
        }
    }
}
