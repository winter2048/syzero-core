using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Runtime.Security;
using SyZero.Runtime.Session;
using SyZero.Util;

namespace SyZero.AspNetCore.Middleware
{
    /// <summary>
    /// 权限中间件
    /// </summary>
    public class SyAuthMiddleware : IMiddleware
    {
        public IToken _token;

        public SyAuthMiddleware(IToken token)
        {
            this._token = token;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.User = null;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string token = context.Request.Headers["Authorization"];

                var Principal = _token.GetPrincipal(token);

                if (Principal != null)
                {
                    context.User = Principal;
                }
            }
            await next.Invoke(context);
        }
    }
}
