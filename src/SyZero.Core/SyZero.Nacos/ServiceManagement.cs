using Nacos.V2;
using Nacos.V2.Naming.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SyZero.Cache;
using SyZero.Runtime.Security;
using SyZero.Service;
using ServiceInfo = SyZero.Service.ServiceInfo;

namespace SyZero.Nacos
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly INacosNamingService _nacosNamingService;
        private readonly ICache _cache;

        public ServiceManagement(INacosNamingService nacosNamingService, ICache cache)
        {
            this._nacosNamingService = nacosNamingService;
            this._cache = cache;
        }

        public async Task<List<ServiceInfo>> GetService(string serviceName)
        {
            if (_cache.Exist($"Consul:{serviceName}"))
            {
                return _cache.Get<List<ServiceInfo>>($"Consul:{serviceName}");
            }
            else
            {
                // 这里需要知道被调用方的服务名
                var services = await _nacosNamingService.SelectInstances(serviceName, true);
                if (services.Count == 0)
                {
                    throw new Exception($"SyZero.Consul:未找到{serviceName}服务!");
                }
                var serviceInfos = services.Select(service => new ServiceInfo()
                {
                    ServiceID = service.InstanceId,
                    ServiceName = service.ServiceName,
                    ServiceAddress = service.Ip,
                    ServicePort = service.Port,
                    ServiceProtocol = service.Metadata.TryGetValue("secure", out _) ? ProtocolType.HTTPS : ProtocolType.HTTP
                }).ToList();
                _cache.Set($"Consul:{serviceName}", serviceInfos, 30);
                return serviceInfos;
            }
        }
    }
}
