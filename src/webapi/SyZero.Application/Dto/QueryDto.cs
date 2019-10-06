using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Application
{
    public class QueryDto
    {
            /// <summary>
            /// 定位
            /// </summary>
            public string offset { get; set; }
            /// <summary>
            /// 页大小
            /// </summary>
            public string limit { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public string sort { get; set; }
            /// <summary>
            /// 筛选
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// Tc
            /// </summary>
            public string tc { get; set; }
    }
}
