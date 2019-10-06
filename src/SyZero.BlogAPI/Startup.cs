using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;
using SyZero.Application;
using SyZero.EntityFrameworkCore;
using SyZero.MongoDB;

namespace SyZero.BlogAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = Configuration["JWT:audience"],//Audience
                        ValidIssuer = Configuration["JWT:issuer"],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]))//拿到SecurityKey
                    };
                });
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
       
            #region Swagger
            services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new Info
                  {
                      Version = "v1",
                      Title = "SyBlog接口文档",
                      Description = "RESTful API for SyBlogManagement",
                      TermsOfService = "None",
                      Contact = new Contact { Name = "SYZERO", Email = "522112669@qq.com", Url = "http://syzero.com" }
                  });

                  c.OperationFilter<AuthTokenHeaderParameter>();
                  var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）PlatformServices.Default.Application.ApplicationBasePath;
                  var xmlPath = Path.Combine(basePath, "syBlogApi.xml");
                  c.IncludeXmlComments(xmlPath);
                  //  c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
              });
            #endregion
            var builder = new ContainerBuilder();//实例化 AutoFac  容器            
            builder.Populate(services);

            //模块注册
            //使用EF仓储
            builder.RegisterModule(new EntityFrameworkModule());
            //使用Mongodb仓储
            builder.RegisterModule(new MongoDBModule());
            //注入Application程序集
            builder.RegisterModule(new AutoFacApplicationModule());
            //注入DomainService程序集
            builder.RegisterModule(new AutoFacDomaninServiceModule());
          
            return new AutofacServiceProvider(builder.Build());//第三方IOC接管 core内置DI容器
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            // app.UseHttpsRedirection();   //使用https

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyBlogManagement API V1");
                c.RoutePrefix = string.Empty;
            });

            //cors跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
            });


            app.UseMvc();
        }
    }
}
