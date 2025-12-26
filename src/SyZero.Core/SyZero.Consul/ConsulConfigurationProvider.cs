using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

namespace SyZero.Consul
{
    public sealed class ConsulConfigurationProvider : ConfigurationProvider
    {
        private readonly ConsulConfigurationParser configurationParser;
        private readonly IConsulConfigurationSource source;

        public ConsulConfigurationProvider(IConsulConfigurationSource source, ConsulConfigurationParser configurationParser)
        {
            this.configurationParser = configurationParser;
            this.source = source;

            if (source.ReloadOnChange)
            {
                ChangeToken.OnChange(
                    () => this.configurationParser.Watch(this.source.ServiceKey, this.source.CancellationToken),
                    async () =>
                    {
                        Console.WriteLine("--------- SyZero.Consul：检测到配置变更 => 开始重新加载配置");
                        await this.configurationParser.GetConfig(true, source).ConfigureAwait(false);
                        Thread.Sleep(source.ReloadDelay);
                        this.Load();
                        Console.WriteLine("--------- SyZero.Consul：检测到配置变更 => 完成");
                        this.OnReload();
                    });
            }
        }

        public override void Load()
        {
            try
            {
                this.Data = this.configurationParser.GetConfig(false, this.source).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (AggregateException aggregateException)
            {
                if (aggregateException.InnerException != null)
                {
                    throw aggregateException.InnerException;
                }

                throw;
            }
        }
    }
}