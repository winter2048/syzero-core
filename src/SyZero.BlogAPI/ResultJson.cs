using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyZero.BlogAPI
{
    public class ResultJson
    {
        /// <summary>
        /// 401未认证 200正常  404null
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        public ResultJson(string Msg, object Data = null, int Code = 200, int Total = 0)
        {
            this.msg = Msg;
            this.data = Data;
            this.code = Code;
            this.total = Total;
        }
    }
}
