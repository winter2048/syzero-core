using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SyZero.Util;

namespace Microsoft.AspNetCore.Builder
{
    public static class SyZeroExtensions
    {
        public static IApplicationBuilder UseSyZero(this IApplicationBuilder app)
        {
            #region Autofac依赖注入服务
            AutofacUtil.Container = app.ApplicationServices.GetAutofacRoot();
            #endregion

            System.Console.WriteLine("启动成功");
            return app;
        }
    }
}
