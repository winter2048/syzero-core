using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyZero.Service
{
    public interface IServiceManagement
    {
        /// <summary>
        /// 根据服务名称获取服务实例
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<List<ServiceInfo>> GetService(string serviceName);
    }
}
