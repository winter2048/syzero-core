using Newtonsoft.Json;
using System;

namespace SyZero.Web.Common
{
    /// <summary>
    /// Long转Str
    /// </summary>
    public class LongToStrConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 由于CanConvert过滤，数据类型只可能是long或ulong
            // 统一转换成long类型处理
            long v = value is ulong ? (long)(ulong)value : (long)value;
            writer.WriteValue(v.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // 取得读到的字符串
            string hex = reader.Value as string;
            if (String.IsNullOrEmpty(hex))
            {
                return null;
            }
            // 调用ToInt64扩展将字符串转换成long型
            long value = hex.ToLong();
            // 将v转换成实际需要的类型 ulong 或 long(不转换)
            return typeof(ulong) == objectType ? (object)(ulong)value : value;
        }

        public override bool CanConvert(Type objectType)
        {
            // 只处理long和ulong两种类型的数据
            if (objectType == typeof(Int64?) || objectType == typeof(Int64) || objectType == typeof(UInt64?) || objectType == typeof(UInt64))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}