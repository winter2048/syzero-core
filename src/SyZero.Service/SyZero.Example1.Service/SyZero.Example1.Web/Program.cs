using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using SyZero.Example1.Core.DbContext;
using SyZero.Example1.Web.Core.Filter;
using SyZero.DynamicWebApi;
using SyZero.Web.Common;

namespace SyZero.Example1.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var builder = WebApplication.CreateBuilder(args);

            //使用SyZero
            builder.AddSyZero();

            //builder.Configuration.AddNacos(cancellationTokenSource.Token); //Nacos动态配置
            //builder.Configuration.AddConsul(cancellationTokenSource.Token); //Consul动态配置

            builder.WebHost.UseUrls($"{AppConfig.ServerOptions.Protocol}://*:{AppConfig.ServerOptions.Port}");

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            }).AddSyZeroLog4Net();

            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            builder.Services.AddControllers().AddMvcOptions(options =>
            {
                options.Filters.Add(new AppExceptionFilter());
                options.Filters.Add(new AppResultFilter());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new LongToStrConverter());
            });

            //动态WebApi
            builder.Services.AddDynamicWebApi(new DynamicWebApiOptions()
            {
                DefaultApiPrefix = "/api",
                DefaultAreaName = AppConfig.ServerOptions.Name
            });
            //Swagger
            builder.Services.AddSwagger();
            //使用OpenTelemetry遥测
            //builder.Services.AddSyZeroOpenTelemetry();
            //使用AutoMapper
            builder.Services.AddSyZeroAutoMapper();
            //使用SqlSugar仓储
            builder.Services.AddSyZeroSqlSugar<Example1DbContext>();
            //注入控制器
            builder.Services.AddSyZeroController();
            //注入公共层
            builder.Services.AddSyZeroCommon();
            //注入Consul
            //builder.Services.AddConsul();
            //注入Feign
            builder.Services.AddSyZeroFeign();

            var app = builder.Build();

            app.UseSyZero();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.UseRouting();
            app.UseStaticFiles();
            app.UseSyAuthMiddleware((sySeesion) => "Token:" + sySeesion.UserId);
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyZero.Example1.Web API V1");
                c.RoutePrefix = "api/swagger";

            });
            app.InitTables();
            app.Run();
        }
    }
}
