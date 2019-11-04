using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SyZero.Web.Common
{
    /// <summary>
    /// Json序列化
    /// </summary>
    interface IJsonSerialize
    {
        /// <summary>
        /// Obj序列化
        /// </summary>
        /// <param name="obj">Obj</param>
        /// <returns></returns>
        string ObjectToJSON(object obj);

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonText">Json字符串</param>
        /// <returns></returns>
        T JSONToObject<T>(string jsonText);
    }
}
