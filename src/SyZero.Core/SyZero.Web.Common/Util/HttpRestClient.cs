using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Client;
using SyZero.Util;

namespace SyZero.Web.Common.Util
{
    /// <summary>
    /// HttpRestClient class
    /// </summary>
    public class HttpRestClient : IClient
    {
        public async Task<ResponseTemplate<T>> ExecuteAsync<T>(RequestTemplate requestTemplate, CancellationToken cancellationToken)
        {
            var client = SyZeroUtil.GetService<RestClient>();
            var requset = new RestRequest(requestTemplate.Url, GetMethod(requestTemplate));
            requset.AddHeaders(requestTemplate.Headers);
            requset.AddJsonBody(requestTemplate.Body ?? "");
            foreach (var item in requestTemplate.QueryValue)
            {
                requset.AddQueryParameter(item.Key, item.Value);
            }
            var response = await client.ExecuteAsync(requset, cancellationToken);
            return GetResponseTemplate<T>(response);
        }


        private Method GetMethod(RequestTemplate requestTemplate) {
            Method method = Method.Get;
            if (requestTemplate.HttpMethod == HttpMethod.Post)
            {
                method = Method.Post;
            }
            else if (requestTemplate.HttpMethod == HttpMethod.Put)
            {
                method = Method.Put;
            }
            else if (requestTemplate.HttpMethod == HttpMethod.Delete)
            {
                method = Method.Delete;
            }
            else if (requestTemplate.HttpMethod == HttpMethod.Get)
            {
                method = Method.Get;
            }
            return method;
        }

        private ResponseTemplate<T> GetResponseTemplate<T>(RestResponse response)
        {
            var responseTemplate = new ResponseTemplate<T>();
            responseTemplate.HttpStatusCode = response.StatusCode;
            if (response.IsSuccessful)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Content);
                responseTemplate.Code = data.code;
                if (data.code == (int)SyMessageBoxStatus.Success)
                {
                    string jsonStr = data.data.ToString();
                    var ddd = typeof(T);
                    if (typeof(T) == typeof(string))
                    {
                        responseTemplate.Body = data.data;
                    }
                    else
                    {
                        responseTemplate.Body = JsonConvert.DeserializeObject<T>(jsonStr);
                    }
                }
                else
                {
                    responseTemplate.Msg = data.msg;
                }
            }
            return responseTemplate;
        }
    }
}
