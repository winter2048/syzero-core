using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace SyZero.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot()//Ocelot如何处理
                .AddConsul()//支持Consul
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();//默认字典存储
                })
                .AddPolly();

        }

        //public void ConfigureContainer(ContainerBuilder builder)
        //{

        //    //使用SyZero
        //    builder.RegisterModule(new SyZeroModule());

        //}


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();//请求交给Ocelot处理
        }
    }
}
