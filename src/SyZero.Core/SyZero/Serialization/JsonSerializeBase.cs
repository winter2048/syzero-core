using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Dependency;

namespace SyZero.Serialization
{
    public class JsonSerializeBase : IJsonSerialize
    {
        public T JSONToObject<T>(string jsonText)
        {
            throw new NotImplementedException();
        }

        public string ObjectToJSON(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
