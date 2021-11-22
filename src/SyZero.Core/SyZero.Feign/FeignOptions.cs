using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Feign
{
    /// <summary>
    /// 配置
    /// </summary>
    public class FeignOptions
    {
        /// <summary>
        /// 服务
        /// </summary>
        public List<FeignService> Service { get; set; } = new List<FeignService>();
        /// <summary>
        /// 全局配置
        /// </summary>
        public ServiceSetting Global { get; set; }
    }


    public class FeignService : ServiceSetting
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// DLL名称
        /// </summary>
        public string DllName { get; set; }
    }

    public class ServiceSetting
    {
        /// <summary>
        /// 策略
        /// </summary>
        public string Strategy { get; set; }
        /// <summary>
        /// 重试次数
        /// </summary>
        public int Retry { get; set; } = 0;
    }
}
