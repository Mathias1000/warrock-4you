using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Warrock
{
    public class ServiceInfo
    {
        public int Port { get; set; }
        public IPAddress IP { get; set; }
        public int ServerID { get; set; }
        public string ServicePassword { get; set; }
    }
}
