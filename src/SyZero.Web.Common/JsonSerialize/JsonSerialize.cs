using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Web.Common
{
    public class JsonSerialize : IJsonSerialize
    {
        public T JSONToObject<T>(string jsonText)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        public string ObjectToJSON(object obj)
        {
            try
            {
                byte[] b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
                return Encoding.UTF8.GetString(b);
            }
            catch (Exception ex)
            {

                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
    }
}
