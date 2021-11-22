using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyZero.Web.Common
{
    public class RestHelper
    {
        /// <summary>
        /// 通过传入的请求信息访问服务端，并返回结果对象
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="request">外部设定的请求</param>
        /// <returns>返回Jobject通用对象</returns>
        public static JObject Execute(string baseUrl, RestRequest request)
        {
            var client = new RestClient(baseUrl);
            //client.BaseUrl = new Uri(baseUrl);
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                return null;
            }
            //返回的内容为Html则返回不对象化
            if (response.Content.Contains("<html xmlns="))
            {
                return null;
            }
            return JObject.Parse(response.Content);
        }

        /// <summary>
        /// 通过传入的请求信息访问服务端，并返回T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUrl"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static T Execute<T>(string baseUrl, RestRequest request) where T : class, new()
        {
            var client = new RestClient(baseUrl);
            //client.BaseUrl = new Uri(baseUrl);
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public static string ExecuteToString(string baseUrl, RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                return string.Empty;
            }

            return response.Content;
        }

        /// <summary>
        /// PostJson数据 无返回值
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="resource"></param>
        /// <param name="postData"></param>
        public static string PostJson(string baseUrl, string resource, dynamic postData)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(postData); // uses JsonSerializer

            var response = client.Execute(request);
            return response.Content;
        }

        /// <summary>
        /// PostJson数据，返回值T
        /// </summary>
        /// <returns></returns>
        public static HttpResultMessage<T> PostJson<T>(string baseUrl, string resource, dynamic postData, string token = "")
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", token);
            request.AddBody(postData); // uses JsonSerializer


            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            //if (!string.IsNullOrWhiteSpace(content))
            //{
            //    content = content.Replace("]\"", "]").Replace("\"[", "[");
            //    content = content.Replace("}\"", "}").Replace("\"{", "{").Replace("\\\"", "\"");
            //}
            var result = new HttpResultMessage<T>();
            var resultStr = JsonConvert.DeserializeObject<HttpResultMessage<object>>(content);
            //Console.WriteLine($"PostJson baseUrl{baseUrl} resource{resource} postData{JsonConvert.SerializeObject(postData)} content:{content}");
            result.IsSucceed = resultStr.IsSucceed;
            if (resultStr.IsSucceed)
            {
                var remoteInvokeResult = resultStr.Entity;
                if (remoteInvokeResult != null)
                {
                    if (remoteInvokeResult.GetType().IsValueType)
                    {
                        //if(typeof(T) == typeof(int))
                        //{ }
                        //result.Entity = (T)remoteInvokeResult;
                        result.Entity = (T)Convert.ChangeType(remoteInvokeResult, typeof(T));
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        result.Entity = (T)remoteInvokeResult;
                    }
                    else
                    {
                        result.Entity = JsonConvert.DeserializeObject<T>(remoteInvokeResult.ToString());
                    }
                    //result.Entity = JsonConvert.DeserializeObject<T>(resultStr.Entity);
                }
                else
                {
                    resultStr.IsSucceed = false;
                    resultStr.Message = "状态值为空！";
                }
            }
            //Console.WriteLine(result.Message);
            return result;
        }

        public static HttpResultMessage PostJsonAsUrl(string baseUrl, string resource, dynamic postData)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(postData); // uses JsonSerializer

            IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string
            //if (!string.IsNullOrWhiteSpace(content))
            //{
            //    content = content.Replace("]\"", "]").Replace("\"[", "[");
            //    content = content.Replace("}\"", "}").Replace("\"{", "{").Replace("\\\"", "\"");
            //}

            return JsonConvert.DeserializeObject<HttpResultMessage>(response.Content);
        }


        public static T EasemobReqUrl<T>(string resource, Method method, dynamic postData, string token = "")
        {
            try
            {

                var client = new RestClient();
                var request = new RestRequest(resource, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.AddHeader("Authorization", "Bearer " + token); }
                // request.AddHeader("Authorization", token);
                if (method != Method.GET)
                {
                    request.AddBody(postData); // uses JsonSerializer
                }


                IRestResponse response = client.Execute(request);
                var resultStr = response.Content;
                var result = default(T);
                //if (remoteInvokeResult.GetType().IsValueType)
                //{
                //    result = (T)Convert.ChangeType(remoteInvokeResult, typeof(T));
                //}
                //else 
                if (typeof(T) == typeof(string))
                {
                    result = (T)Convert.ChangeType(resultStr, typeof(T));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<T>(resultStr);
                }
                return result;

                //HttpWebRequest request = WebRequest.Create(reqUrl) as HttpWebRequest;
                //request.Method = method.ToUpperInvariant();

                //if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.Headers.Add("Authorization", "Bearer " + token); }
                //if (request.Method.Tostring() != "GET" && !string.IsNullOrEmpty(paramData) && paramData.Length > 0)
                //{
                //    request.ContentType = "application/json";
                //    byte[] buffer = Encoding.UTF8.GetBytes(paramData);
                //    request.ContentLength = buffer.Length;
                //    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                //}

                //using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                //{
                //    using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                //    {
                //        string result = stream.ReadToEnd();
                //        return result;
                //    }
                //}
            }
            catch (Exception ex) { return default(T); }
        }


        public static T WechatPost<T>(string resource, dynamic postData, string token = "")
        {
            try
            {

                var client = new RestClient();
                var request = new RestRequest(resource, Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.AddHeader("Authorization", "Bearer " + token); }
                // request.AddHeader("Authorization", token);
                request.AddBody(postData); // uses JsonSerializer



                IRestResponse response = client.Execute(request);
                var resultStr = response.Content;
                var result = default(T);
                //if (remoteInvokeResult.GetType().IsValueType)
                //{
                //    result = (T)Convert.ChangeType(remoteInvokeResult, typeof(T));
                //}
                //else 
                if (typeof(T) == typeof(string))
                {
                    result = (T)Convert.ChangeType(resultStr, typeof(T));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<T>(resultStr);
                }
                return result;

            }
            catch (Exception ex) { return default(T); }
        }

        public static T WechatGet<T>(string resource, string token = "")
        {
            try
            {

                var client = new RestClient();
                var request = new RestRequest(resource, Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.AddHeader("Authorization", token); }
                // request.AddHeader("Authorization", token);



                IRestResponse response = client.Execute(request);
                var resultStr = response.Content;
                var result = default(T);
                //if (remoteInvokeResult.GetType().IsValueType)
                //{
                //    result = (T)Convert.ChangeType(remoteInvokeResult, typeof(T));
                //}
                //else 
                if (typeof(T) == typeof(string))
                {
                    result = (T)Convert.ChangeType(resultStr, typeof(T));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<T>(resultStr);
                }
                return result;

            }
            catch (Exception ex) { return default(T); }
        }
        public async static Task<string> RestPost(string url, Dictionary<string, string> header = null, Dictionary<string, string> parameter = null, string body = "")
        {
            return await Request(url, header, parameter, body, Method.POST);
        }
        public async static Task<string> RestGet(string url, Dictionary<string, string> header = null, Dictionary<string, string> parameter = null)
        {
            return await Request(url, header, parameter, string.Empty, Method.GET);
        }
        private async static Task<string> Request(string url, Dictionary<string, string> header, Dictionary<string, string> parameter, string body, Method method)
        {
            RestClient client = new RestClient();
            string str;
            try
            {
                var uri = new Uri(url);
                RestRequest request = new RestRequest(new Uri(url), method)
                {
                    //request.Timeout = 2000;
                    RequestFormat = DataFormat.Json
                };
                if (header != null)
                {
                    foreach (var item in header.Keys)
                    {
                        request.AddHeader(item, header[item]);
                    }
                }
                if (parameter != null)
                {
                    foreach (var item in parameter.Keys)
                    {
                        request.AddParameter(item, parameter[item]);
                    }
                }
                if (!string.IsNullOrEmpty(body))
                {
                    request.AddJsonBody(body);
                }
                var response = await client.ExecuteAsync(request);
                str = response.Content;
                if (string.IsNullOrEmpty(str))
                {
                    str = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return str;
        }

    }
}
