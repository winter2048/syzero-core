using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Application.Routing;

namespace SyZero.Client
{
    public class RequestTemplate
    {
        public HttpMethod HttpMethod { get; set; }

        public string Url { get; set; }
        /// <summary>
        /// 判断是否为表单的post提交
        /// </summary>
        public bool IsForm { get; set; }
        public string Body { get; set; }

        public List<KeyValuePair<string, string>> FormValue { set; get; } = new List<KeyValuePair<string, string>>();
        public IDictionary<string, IList<string>> Headers { get; set; } = new Dictionary<string, IList<string>>();

        public RequestTemplate()
        {
        }

        public RequestTemplate(HttpMethod httpMethod, string url)
        {
            HttpMethod = httpMethod;
            Url = url;
        }
    }
}
