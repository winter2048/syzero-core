using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading;
using SyZero.AspNetCore;
using SyZero.AspNetCore.Middleware;
using SyZero.Cache;
using SyZero.Runtime.Session;
using SyZero.Util;

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
        public static IApplicationBuilder UseSyAuthMiddleware(this IApplicationBuilder app, Func<ISySession, string> cacheKeyFun = null)
        {
            Console.WriteLine("注入权限中间件...");
            app.UseMiddleware<SyAuthMiddleware>();
            if (cacheKeyFun != null)
            {
                app.Use(async (context, next) =>
                {
                    var sySeesion = AutofacUtil.GetService<ISySession>();
                    if (sySeesion.UserId != null)
                    {
                        var cache = AutofacUtil.GetService<ICache>();
                        if (!cache.Exist(cacheKeyFun(sySeesion)))
                        {
                            Thread.CurrentPrincipal = null;
                            context.User = null;
                        }
                    }
                    await next.Invoke();
                });
            }
            return app;
        }
    }
}
