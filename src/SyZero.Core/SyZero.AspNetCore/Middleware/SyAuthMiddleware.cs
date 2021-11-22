using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Runtime.Security;

namespace SyZero.AspNetCore.Middleware
{
    /// <summary>
    /// 权限中间件
    /// </summary>
    public class SyAuthMiddleware : IMiddleware
    {
        private IToken _token;

        public SyAuthMiddleware(IToken token)
        {
            this._token = token;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Thread.CurrentPrincipal = null;
            context.User = null;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string token = context.Request.Headers["Authorization"];

                var Principal = _token.GetPrincipal(token);

                if (Principal != null)
                {
                    Thread.CurrentPrincipal = Principal;
                    context.User = Principal;
                }
            }
            await next.Invoke(context);
        }
    }
}
