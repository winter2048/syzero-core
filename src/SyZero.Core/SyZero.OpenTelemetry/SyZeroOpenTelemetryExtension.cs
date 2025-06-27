using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SyZero
{
    public static class SyZeroOpenTelemetryExtension
    {
        public static IServiceCollection AddSyZeroOpenTelemetry(this IServiceCollection services)
        {
            var resource = ResourceBuilder.CreateDefault().AddService(AppConfig.ServerOptions.Name);
            services.AddOpenTelemetry()
              .WithTracing(b => b.SetResourceBuilder(resource)
                  .AddSource("*")
                  .AddAspNetCoreInstrumentation(opt =>
                  {
                      opt.Filter = context =>
                      {
                          return context.Request.Path.ToString().StartsWith("/api/");
                      };
                  })
                  .AddHttpClientInstrumentation(opt =>
                  {
                      opt.FilterHttpWebRequest = context =>
                      {
                          return context.RequestUri.AbsolutePath.ToString().StartsWith("/api/");
                      };
                  }))
              .WithMetrics(b => b.SetResourceBuilder(resource)
                  .AddMeter("*")
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation())
              .WithLogging()
              .UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.Grpc, new System.Uri(AppConfig.Configuration.GetValue<string>("OpenTelemetry:OtlpUrl")));
            return services;
        }
    }
}
