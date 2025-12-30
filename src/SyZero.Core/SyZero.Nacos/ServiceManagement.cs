using Nacos.V2;
using Nacos.V2.Naming.Dtos;
using Nacos.V2.Naming.Event;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<string, ServiceChangeListener> _listeners = new ConcurrentDictionary<string, ServiceChangeListener>();
        private readonly Random _random = new Random();

        public ServiceManagement(INacosNamingService nacosNamingService, ICache cache)
        {
            this._nacosNamingService = nacosNamingService;
            this._cache = cache;
        }

        #region 服务查询

        public async Task<List<ServiceInfo>> GetService(string serviceName)
        {
            if (_cache.Exist($"Nacos:{serviceName}"))
            {
                return _cache.Get<List<ServiceInfo>>($"Nacos:{serviceName}");
            }
            else
            {
                // 这里需要知道被调用方的服务名
                var services = await _nacosNamingService.SelectInstances(serviceName, true);
                if (services.Count == 0)
                {
                    throw new Exception($"SyZero.Nacos:未找到{serviceName}服务!");
                }
                var serviceInfos = services.Select(service => MapToServiceInfo(service)).ToList();
                _cache.Set($"Nacos:{serviceName}", serviceInfos, 30);
                return serviceInfos;
            }
        }

        public async Task<List<ServiceInfo>> GetHealthyServices(string serviceName)
        {
            var services = await _nacosNamingService.SelectInstances(serviceName, true);
            if (services == null || services.Count == 0)
            {
                return new List<ServiceInfo>();
            }
            return services.Select(service => MapToServiceInfo(service)).ToList();
        }

        public async Task<ServiceInfo> GetServiceInstance(string serviceName)
        {
            var services = await GetHealthyServices(serviceName);
            if (services == null || services.Count == 0)
            {
                throw new Exception($"SyZero.Nacos:未找到可用的{serviceName}服务实例!");
            }
            // 简单随机负载均衡
            var index = _random.Next(services.Count);
            return services[index];
        }

        public async Task<List<string>> GetAllServices()
        {
            var servicesInfo = await _nacosNamingService.GetServicesOfServer(1, int.MaxValue);
            return servicesInfo?.Data?.ToList() ?? new List<string>();
        }

        #endregion

        #region 服务注册/注销

        public async Task RegisterService(ServiceInfo serviceInfo)
        {
            var metadata = serviceInfo.Metadata ?? new Dictionary<string, string>();
            metadata["Protocol"] = serviceInfo.ServiceProtocol.ToString();
            metadata["secure"] = serviceInfo.ServiceProtocol == ProtocolType.HTTPS ? "true" : "false";
            if (!string.IsNullOrEmpty(serviceInfo.Version))
                metadata["Version"] = serviceInfo.Version;
            if (!string.IsNullOrEmpty(serviceInfo.Group))
                metadata["Group"] = serviceInfo.Group;
            if (!string.IsNullOrEmpty(serviceInfo.Region))
                metadata["Region"] = serviceInfo.Region;
            if (!string.IsNullOrEmpty(serviceInfo.Zone))
                metadata["Zone"] = serviceInfo.Zone;
            if (!string.IsNullOrEmpty(serviceInfo.HealthCheckUrl))
                metadata["HealthCheckUrl"] = serviceInfo.HealthCheckUrl;
            if (serviceInfo.HealthCheckIntervalSeconds > 0)
                metadata["HealthCheckIntervalSeconds"] = serviceInfo.HealthCheckIntervalSeconds.ToString();
            if (serviceInfo.HealthCheckTimeoutSeconds > 0)
                metadata["HealthCheckTimeoutSeconds"] = serviceInfo.HealthCheckTimeoutSeconds.ToString();
            metadata["RegisterTime"] = (serviceInfo.RegisterTime != default ? serviceInfo.RegisterTime : DateTime.UtcNow).ToString("O");

            var instance = new Instance
            {
                InstanceId = serviceInfo.ServiceID,
                ServiceName = serviceInfo.ServiceName,
                Ip = serviceInfo.ServiceAddress,
                Port = serviceInfo.ServicePort,
                Healthy = serviceInfo.IsHealthy,
                Enabled = serviceInfo.Enabled,
                Weight = serviceInfo.Weight,
                Metadata = metadata,
                ClusterName = serviceInfo.Group
            };
            await _nacosNamingService.RegisterInstance(serviceInfo.ServiceName, instance);
        }

        public async Task DeregisterService(string serviceId)
        {
            // Nacos需要通过服务名和实例信息来注销
            // 由于只有serviceId，需要先查询服务信息
            // 这里假设serviceId格式为: serviceName#ip#port
            var parts = serviceId.Split('#');
            if (parts.Length >= 3)
            {
                var serviceName = parts[0];
                var ip = parts[1];
                var port = int.Parse(parts[2]);
                await _nacosNamingService.DeregisterInstance(serviceName, ip, port);
            }
            else
            {
                throw new ArgumentException($"无效的serviceId格式: {serviceId}，期望格式为: serviceName#ip#port");
            }
        }

        #endregion

        #region 健康检查

        public async Task<bool> IsServiceHealthy(string serviceName)
        {
            var healthyServices = await GetHealthyServices(serviceName);
            return healthyServices != null && healthyServices.Count > 0;
        }

        #endregion

        #region 服务订阅

        public async Task Subscribe(string serviceName, Action<List<ServiceInfo>> callback)
        {
            var listener = new ServiceChangeListener(callback);
            _listeners[serviceName] = listener;
            await _nacosNamingService.Subscribe(serviceName, listener);
        }

        public async Task Unsubscribe(string serviceName)
        {
            if (_listeners.TryRemove(serviceName, out var listener))
            {
                await _nacosNamingService.Unsubscribe(serviceName, listener);
            }
        }

        #endregion

        #region 辅助方法

        private static ServiceInfo MapToServiceInfo(Instance instance)
        {
            var protocol = instance.Metadata.TryGetValue("secure", out var secure) && secure == "true" 
                ? ProtocolType.HTTPS : ProtocolType.HTTP;
            instance.Metadata.TryGetValue("Version", out var version);
            instance.Metadata.TryGetValue("Group", out var group);
            instance.Metadata.TryGetValue("Region", out var region);
            instance.Metadata.TryGetValue("Zone", out var zone);
            instance.Metadata.TryGetValue("HealthCheckUrl", out var healthCheckUrl);
            int.TryParse(instance.Metadata.TryGetValue("HealthCheckIntervalSeconds", out var intervalStr) ? intervalStr : "10", out var healthCheckInterval);
            int.TryParse(instance.Metadata.TryGetValue("HealthCheckTimeoutSeconds", out var timeoutStr) ? timeoutStr : "5", out var healthCheckTimeout);
            DateTime.TryParse(instance.Metadata.TryGetValue("RegisterTime", out var registerTimeStr) ? registerTimeStr : null, out var registerTime);

            return new ServiceInfo
            {
                ServiceID = instance.InstanceId,
                ServiceName = instance.ServiceName,
                ServiceAddress = instance.Ip,
                ServicePort = instance.Port,
                ServiceProtocol = protocol,
                Version = version,
                Group = group ?? instance.ClusterName,
                Metadata = instance.Metadata != null ? new Dictionary<string, string>(instance.Metadata) : new Dictionary<string, string>(),
                IsHealthy = instance.Healthy,
                Enabled = instance.Enabled,
                Weight = instance.Weight,
                RegisterTime = registerTime,
                Region = region,
                Zone = zone,
                HealthCheckUrl = healthCheckUrl,
                HealthCheckIntervalSeconds = healthCheckInterval,
                HealthCheckTimeoutSeconds = healthCheckTimeout
            };
        }

        #endregion

        /// <summary>
        /// 服务变更监听器
        /// </summary>
        private class ServiceChangeListener : IEventListener
        {
            private readonly Action<List<ServiceInfo>> _callback;

            public ServiceChangeListener(Action<List<ServiceInfo>> callback)
            {
                _callback = callback;
            }

            public Task OnEvent(IEvent @event)
            {
                if (@event is InstancesChangeEvent changeEvent)
                {
                    var services = changeEvent.Hosts?.Select(instance => MapToServiceInfo(instance)).ToList() ?? new List<ServiceInfo>();
                    _callback?.Invoke(services);
                }
                return Task.CompletedTask;
            }
        }
    }
}
