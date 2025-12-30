using Consul;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Provider.Consul.Interfaces;
using Ocelot.Provider.Consul;

namespace SyZero.Gateway
{
    public class ConsulServiceBuilder : DefaultConsulServiceBuilder
    {
        public ConsulServiceBuilder(IHttpContextAccessor contextAccessor, IConsulClientFactory clientFactory, IOcelotLoggerFactory loggerFactory)
            : base(contextAccessor, clientFactory, loggerFactory) { }

        // I want to use the agent service IP address as the downstream hostname
        protected override string GetDownstreamHost(ServiceEntry entry, Node node)
            => entry.Service.Address;
    }
}
