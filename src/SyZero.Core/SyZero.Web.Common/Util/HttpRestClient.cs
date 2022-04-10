using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Client;

namespace SyZero.Web.Common.Util
{
    /// <summary>
    /// HttpRestClient class
    /// </summary>
    public class HttpRestClient : IClient
    {
        public async Task<ResponseTemplate<T>> ExecuteAsync<T>(RequestTemplate requestTemplate, CancellationToken cancellationToken)
        {        
            var client = new RestClient();
            var requset = new RestRequest(requestTemplate.Url, GetMethod(requestTemplate));
            requset.AddHeaders(requestTemplate.Headers);
            var response = await client.ExecuteAsync(requset, cancellationToken);
            return GetResponseTemplate<T>(response);
        }


        private Method GetMethod(RequestTemplate requestTemplate) {
            Method method;
            switch (requestTemplate.HttpMethod)
            {
                case Application.Routing.HttpMethod.POST:
                    method = Method.POST;
                    break;
                case Application.Routing.HttpMethod.PUT:
                    method = Method.PUT;
                    break;
                case Application.Routing.HttpMethod.DELETE:
                    method = Method.DELETE;
                    break;
                case Application.Routing.HttpMethod.GET:
                    method = Method.GET;
                    break;
                default:
                    method = Method.GET;
                    break;
            }
            return method;
        }

        private ResponseTemplate<T> GetResponseTemplate<T>(IRestResponse response)
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
                    responseTemplate.Body = JsonConvert.DeserializeObject<T>(jsonStr);
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
