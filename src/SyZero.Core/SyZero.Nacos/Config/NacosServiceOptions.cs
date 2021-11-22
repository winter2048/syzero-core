using System.Collections.Generic;

namespace SyZero.Nacos.Config
{
    /// <summary>
    ///  Nacos配置模型类
    /// </summary>
    public class NacosServiceOptions
    {
        /// <summary>
        /// 服务注册地址（Nacos的地址）
        /// </summary>
        public List<string> NacosAddresses { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public bool Optional { get; set; }

    }
}
