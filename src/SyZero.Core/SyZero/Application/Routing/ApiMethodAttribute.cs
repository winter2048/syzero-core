using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Application.Routing
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiMethodAttribute : Attribute
    {
        public ApiMethodAttribute(HttpMethod httpMethod)
        {
            this.HttpMethod = httpMethod;
        }
      
        public ApiMethodAttribute(HttpMethod httpMethod, string template)
        {
            this.HttpMethod = httpMethod;
            this.Template = template;   
        }

        public HttpMethod HttpMethod { get; }
        public string Template { get; }
    }
}
