using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Redis
{
    public class RedisOptions
    {
        public RedisType Type { get; set; }

        public string Master { get; set; }

        public List<string> Slave { get; set; } = new List<string>();

        public List<string> Sentinel { get; set; } = new List<string>();
    }


    public enum RedisType
    {
        MasterSlave = 0,
        Sentinel = 1,
        Cluster = 2
    }
}
