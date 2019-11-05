using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Core;
namespace SyZero.Domain.Entities
{
    
    public class SYID
    {
        private static IdWorker IdWorker = new IdWorker(1, 1);
        public static long NextId()
        {
            return IdWorker.NextId();
        }

    }
}
