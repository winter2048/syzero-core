using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using SyZero.AspNetCore;
using SyZero.AspNetCore.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routeAttribute"></param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }

        /// <summary>
        /// 权限中间件 - 扩展方法
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseSyAuthMiddleware(this IApplicationBuilder app)
        {
            Console.WriteLine("注入权限中间件...");
            app.UseMiddleware<SyAuthMiddleware>();
            return app;
        }
    }
}
