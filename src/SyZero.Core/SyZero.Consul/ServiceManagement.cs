using NConsul;
using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyZero.Consul.Config;
using SyZero.Runtime.Security;
using SyZero.Service;

namespace SyZero.Consul
{
    public class ServiceManagement : IServiceManagement
    {
        public async Task<List<ServiceInfo>> GetService(string serviceName)
        {
            var consulOptions = AppConfig.GetSection<ConsulServiceOptions>("Consul");
            using (var consulClient = new ConsulClient(configuration =>
            {
                configuration.Address = new Uri(consulOptions.ConsulAddress);
            }))
            {
                var services = await consulClient.Catalog.Service(serviceName);
                if (services.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"SyZero.Consul:Consul连接出错({services.StatusCode})!");
                }
                if (services.Response.Length == 0)
                {
                    throw new Exception($"SyZero.Consul:未找到{serviceName}服务!");
                }
                return services.Response.Select(service => new ServiceInfo()
                {
                    ServiceID = service.ServiceID,
                    ServiceName = service.ServiceName,
                    ServiceAddress = service.ServiceAddress,
                    ServicePort = service.ServicePort,
                    ServiceProtocol = Enum.Parse<ProtocolType>(service.ServiceMeta.FirstOrDefault(meta => meta.Key == "Protocol").Value)
                }).ToList();
            }
        }
    }
}
