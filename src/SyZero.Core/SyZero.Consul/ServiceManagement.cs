using NConsul;
using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly ConcurrentDictionary<string, Action<List<ServiceInfo>>> _subscriptions = new ConcurrentDictionary<string, Action<List<ServiceInfo>>>();
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _watchTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
        private readonly Random _random = new Random();

        public ServiceManagement(IConsulClient consulClient,
            ICache cache)
        {
            _consulClient = consulClient;
            _cache = cache;
        }

        #region 服务查询

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
                        ServiceProtocol = protocol,
                        Tags = service.ServiceTags?.ToList() ?? new List<string>(),
                        Metadata = service.ServiceMeta?.ToDictionary(m => m.Key, m => m.Value) ?? new Dictionary<string, string>(),
                        IsHealthy = true,
                        Enabled = true
                    };
                }).ToList();
                _cache.Set($"Consul:{serviceName}", serviceInfos, 30);
                return serviceInfos;
            }
        }

        public async Task<List<ServiceInfo>> GetHealthyServices(string serviceName)
        {
            var healthChecks = await _consulClient.Health.Service(serviceName, string.Empty, true);
            if (healthChecks.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"SyZero.Consul:Consul连接出错({healthChecks.StatusCode})!");
            }
            if (healthChecks.Response.Length == 0)
            {
                return new List<ServiceInfo>();
            }
            return healthChecks.Response.Select(entry =>
            {
                var service = entry.Service;
                var protocolMeta = service.Meta?.FirstOrDefault(meta => meta.Key == "Protocol");
                var protocol = ProtocolType.HTTP;
                if (protocolMeta.HasValue && !string.IsNullOrEmpty(protocolMeta.Value.Value))
                {
                    Enum.TryParse(protocolMeta.Value.Value, out protocol);
                }
                service.Meta.TryGetValue("Version", out var version);
                service.Meta.TryGetValue("Group", out var group);
                service.Meta.TryGetValue("Region", out var region);
                service.Meta.TryGetValue("Zone", out var zone);
                double.TryParse(service.Meta.TryGetValue("Weight", out var weightStr) ? weightStr : "1", out var weight);
                return new ServiceInfo()
                {
                    ServiceID = service.ID,
                    ServiceName = service.Service,
                    ServiceAddress = service.Address,
                    ServicePort = service.Port,
                    ServiceProtocol = protocol,
                    Version = version,
                    Group = group,
                    Tags = service.Tags?.ToList() ?? new List<string>(),
                    Metadata = service.Meta?.ToDictionary(m => m.Key, m => m.Value) ?? new Dictionary<string, string>(),
                    IsHealthy = true,
                    Enabled = true,
                    Weight = weight > 0 ? weight : 1.0,
                    Region = region,
                    Zone = zone,
                    HealthCheckUrl = $"{protocol.ToString().ToLower()}://{service.Address}:{service.Port}/health"
                };
            }).ToList();
        }

        public async Task<ServiceInfo> GetServiceInstance(string serviceName)
        {
            var services = await GetHealthyServices(serviceName);
            if (services == null || services.Count == 0)
            {
                throw new Exception($"SyZero.Consul:未找到可用的{serviceName}服务实例!");
            }
            // 简单随机负载均衡
            var index = _random.Next(services.Count);
            return services[index];
        }

        public async Task<List<string>> GetAllServices()
        {
            var services = await _consulClient.Catalog.Services();
            if (services.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"SyZero.Consul:Consul连接出错({services.StatusCode})!");
            }
            return services.Response.Keys.ToList();
        }

        #endregion

        #region 服务注册/注销

        public async Task RegisterService(ServiceInfo serviceInfo)
        {
            var meta = serviceInfo.Metadata ?? new Dictionary<string, string>();
            meta["Protocol"] = serviceInfo.ServiceProtocol.ToString();
            if (!string.IsNullOrEmpty(serviceInfo.Version))
                meta["Version"] = serviceInfo.Version;
            if (!string.IsNullOrEmpty(serviceInfo.Group))
                meta["Group"] = serviceInfo.Group;
            if (!string.IsNullOrEmpty(serviceInfo.Region))
                meta["Region"] = serviceInfo.Region;
            if (!string.IsNullOrEmpty(serviceInfo.Zone))
                meta["Zone"] = serviceInfo.Zone;
            meta["Weight"] = serviceInfo.Weight.ToString();
            meta["RegisterTime"] = (serviceInfo.RegisterTime != default ? serviceInfo.RegisterTime : DateTime.UtcNow).ToString("O");

            var healthCheckUrl = !string.IsNullOrEmpty(serviceInfo.HealthCheckUrl)
                ? serviceInfo.HealthCheckUrl
                : $"{serviceInfo.ServiceProtocol.ToString().ToLower()}://{serviceInfo.ServiceAddress}:{serviceInfo.ServicePort}/health";

            var healthCheckInterval = serviceInfo.HealthCheckIntervalSeconds > 0 
                ? serviceInfo.HealthCheckIntervalSeconds 
                : 10;
            var healthCheckTimeout = serviceInfo.HealthCheckTimeoutSeconds > 0 
                ? serviceInfo.HealthCheckTimeoutSeconds 
                : 5;

            var registration = new AgentServiceRegistration
            {
                ID = serviceInfo.ServiceID,
                Name = serviceInfo.ServiceName,
                Address = serviceInfo.ServiceAddress,
                Port = serviceInfo.ServicePort,
                Tags = serviceInfo.Tags?.ToArray(),
                Meta = meta,
                Check = new AgentServiceCheck
                {
                    HTTP = healthCheckUrl,
                    Interval = TimeSpan.FromSeconds(healthCheckInterval),
                    Timeout = TimeSpan.FromSeconds(healthCheckTimeout),
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                }
            };
            var result = await _consulClient.Agent.ServiceRegister(registration);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"SyZero.Consul:服务注册失败({result.StatusCode})!");
            }
        }

        public async Task DeregisterService(string serviceId)
        {
            var result = await _consulClient.Agent.ServiceDeregister(serviceId);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"SyZero.Consul:服务注销失败({result.StatusCode})!");
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

        public Task Subscribe(string serviceName, Action<List<ServiceInfo>> callback)
        {
            _subscriptions[serviceName] = callback;
            
            var cts = new CancellationTokenSource();
            _watchTokens[serviceName] = cts;

            // 启动后台监听任务
            _ = WatchServiceAsync(serviceName, cts.Token);
            
            return Task.CompletedTask;
        }

        public Task Unsubscribe(string serviceName)
        {
            _subscriptions.TryRemove(serviceName, out _);
            
            if (_watchTokens.TryRemove(serviceName, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
            
            return Task.CompletedTask;
        }

        private async Task WatchServiceAsync(string serviceName, CancellationToken cancellationToken)
        {
            ulong lastIndex = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await _consulClient.Health.Service(serviceName, string.Empty, true, new QueryOptions
                    {
                        WaitIndex = lastIndex,
                        WaitTime = TimeSpan.FromMinutes(5)
                    }, cancellationToken);

                    if (result.LastIndex != lastIndex)
                    {
                        lastIndex = result.LastIndex;
                        
                        if (_subscriptions.TryGetValue(serviceName, out var callback))
                        {
                            var services = result.Response.Select(entry =>
                            {
                                var service = entry.Service;
                                var protocolMeta = service.Meta?.FirstOrDefault(meta => meta.Key == "Protocol");
                                var protocol = ProtocolType.HTTP;
                                if (protocolMeta.HasValue && !string.IsNullOrEmpty(protocolMeta.Value.Value))
                                {
                                    Enum.TryParse(protocolMeta.Value.Value, out protocol);
                                }
                                service.Meta.TryGetValue("Version", out var version);
                                service.Meta.TryGetValue("Group", out var group);
                                service.Meta.TryGetValue("Region", out var region);
                                service.Meta.TryGetValue("Zone", out var zone);
                                double.TryParse(service.Meta.TryGetValue("Weight", out var weightStr) ? weightStr : "1", out var weight);
                                return new ServiceInfo()
                                {
                                    ServiceID = service.ID,
                                    ServiceName = service.Service,
                                    ServiceAddress = service.Address,
                                    ServicePort = service.Port,
                                    ServiceProtocol = protocol,
                                    Version = version,
                                    Group = group,
                                    Tags = service.Tags?.ToList() ?? new List<string>(),
                                    Metadata = service.Meta?.ToDictionary(m => m.Key, m => m.Value) ?? new Dictionary<string, string>(),
                                    IsHealthy = true,
                                    Enabled = true,
                                    Weight = weight > 0 ? weight : 1.0,
                                    Region = region,
                                    Zone = zone
                                };
                            }).ToList();
                            
                            callback?.Invoke(services);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception)
                {
                    // 发生错误时等待一段时间后重试
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }

        #endregion
    }
}
