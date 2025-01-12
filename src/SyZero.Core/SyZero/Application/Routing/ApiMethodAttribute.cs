using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SyZero.Application.Routing
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class HttpMethodAttribute : Refit.HttpMethodAttribute
    {
        public HttpMethodAttribute(HttpMethod httpMethod, string Path) : base(RoutingHelper.ApiUrlPre + Path)
        {

            this.httpMethod = httpMethod;
        }

        public HttpMethodAttribute(HttpMethod httpMethod) : this(httpMethod, "")
        {
            this.httpMethod = httpMethod;
        }

        private HttpMethod httpMethod { get; }

        public override HttpMethod Method => httpMethod;
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class GetAttribute : HttpMethodAttribute
    {
        public GetAttribute(string Path) : base(HttpMethod.Get, Path)
        {
        }

        public GetAttribute() : this("")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class PostAttribute : HttpMethodAttribute
    {
        public PostAttribute(string Path) : base(HttpMethod.Post, Path)
        {
        }

        public PostAttribute() : this("")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class PutAttribute : HttpMethodAttribute
    {
        public PutAttribute(string Path) : base(HttpMethod.Put, Path)
        {
        }

        public PutAttribute() : this("")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class DeleteAttribute : HttpMethodAttribute
    {
        public DeleteAttribute(string Path) : base(HttpMethod.Delete, Path)
        {
        }

        public DeleteAttribute() : this("")
        {
        }
    }
}
