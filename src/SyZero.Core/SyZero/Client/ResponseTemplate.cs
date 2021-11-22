using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SyZero.Client
{
    public class ResponseTemplate
    {
        public HttpStatusCode HttpStatusCode { set; get; }

        public Stream Body { set; get; }

        public IDictionary<string, IEnumerable<string>> Headers { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public void EnsureSuccessStatusCode()
        {
            if (this.HttpStatusCode < HttpStatusCode.OK || this.HttpStatusCode > (HttpStatusCode)299) throw new Exception("httpError");
        }
    }
}
