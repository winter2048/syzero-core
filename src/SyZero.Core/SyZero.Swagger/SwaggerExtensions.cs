
using SyZero.Cache;
using SyZero.Configurations;
using SyZero.Util;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Xml.XPath;
using SyZero.Swagger;
using SyZero;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{AppConfig.ServerOptions.Name}接口文档",
                    Description = $"RESTful API for {AppConfig.ServerOptions.Name}"
                });
                options.DocInclusionPredicate((docName, description) => true);
                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Authorization:Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            }
                        },
                        new string[] { }
                    }
                });
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                foreach (FileInfo file in dir.EnumerateFiles("*.xml"))
                {
                    options.OperationFilter<XmlCommentsOperation2Filter>(new XPathDocument(file.FullName));
                }
            });
            return services;
        }
    }
}
