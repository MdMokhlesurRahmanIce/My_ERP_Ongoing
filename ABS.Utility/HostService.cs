using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
 
namespace ABS.Utility
{
   public class HostService
    {
        public static string GetIP()
        {
            //IPHostEntry host;
            //string localIp = "?";
            //string hostName = Dns.GetHostName();
            //host = Dns.GetHostEntry(hostName);
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        localIp = ip.ToString();
            //    }
            //}
            //return localIp;
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

        }
    }
}
