using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Runtime.Security;

namespace SyZero.Service
{
    public class ServiceInfo
    {
        public string ServiceID
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public string ServiceAddress
        {
            get;
            set;
        }
        
        public int ServicePort
        {
            get;
            set;
        }

        public ProtocolType ServiceProtocol
        {
            get;
            set;
        }
    }
}
