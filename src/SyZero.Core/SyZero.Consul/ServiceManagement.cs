using NConsul;
using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyZero.Cache;
using SyZero.Consul.Config;
using SyZero.Runtime.Security;
using SyZero.Service;
using SyZero.Util;

namespace SyZero.Consul
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly IConsulClient _consulClient;
        private readonly ICache _cache;

        public ServiceManagement(IConsulClient consulClient,
            ICache cache)
        {
            _consulClient = consulClient;
            _cache = cache;
        }

        public async Task<List<ServiceInfo>> GetService(string serviceName)
        {
            if (_cache.Exist($"Consul:{serviceName}"))
            {
                return _cache.Get<List<ServiceInfo>>($"Consul:{serviceName}");
            }
            else
            {
                var services = await _consulClient.Catalog.Service(serviceName);
                if (services.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"SyZero.Consul:Consul连接出错({services.StatusCode})!");
                }
                if (services.Response.Length == 0)
                {
                    throw new Exception($"SyZero.Consul:未找到{serviceName}服务!");
                }
                var serviceInfos = services.Response.Select(service => 
                {
                    var protocolMeta = service.ServiceMeta?.FirstOrDefault(meta => meta.Key == "Protocol");
                    var protocol = ProtocolType.HTTP;
                    if (protocolMeta.HasValue && !string.IsNullOrEmpty(protocolMeta.Value.Value))
                    {
                        Enum.TryParse(protocolMeta.Value.Value, out protocol);
                    }
                    return new ServiceInfo()
                    {
                        ServiceID = service.ServiceID,
                        ServiceName = service.ServiceName,
                        ServiceAddress = service.ServiceAddress,
                        ServicePort = service.ServicePort,
                        ServiceProtocol = protocol
                    };
                }).ToList();
                _cache.Set($"Consul:{serviceName}", serviceInfos, 30);
                return serviceInfos;
            }
        }
    }
}
