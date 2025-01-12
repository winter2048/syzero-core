using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SyZero.Serialization;
using SyZero.Util;

namespace SyZero.Feign
{
    public class ResponseFeignHandler : DelegatingHandler
    {
        private string _serverName;
        public ResponseFeignHandler(string serverName, HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
            _serverName = serverName;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 调用基类的方法以获取响应
            var response = await base.SendAsync(request, cancellationToken);

            // 检查响应的内容类型是否为 JSON
            if (response.Content.Headers.ContentType != null &&
                response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonSerialize = SyZeroUtil.GetService<IJsonSerialize>();

                var data = jsonSerialize.JSONToObject<dynamic>(jsonString);
                if (data.code == (int)SyMessageBoxStatus.Success)
                {
                    response.Content = new StringContent(jsonSerialize.ObjectToJSON(data.data));
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                }
            }

            return response;
        }
    }
}
