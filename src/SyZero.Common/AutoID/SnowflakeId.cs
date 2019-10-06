using System;
using System.Collections.Generic;
using System.Text;
using Snowflake;

namespace SyZero.Common
{
    /// <summary>
    /// 雪花算法
    /// </summary>
    public class SnowflakeId
    {
        private static IdWorker worker = new IdWorker(1, 1);
        /// <summary>
        /// 通过雪花算法产生Id
        /// </summary>
        /// <returns></returns>
        public static long GetID()
        {
            return worker.NextId();
        }
    }
}
