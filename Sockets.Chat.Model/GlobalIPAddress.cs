using System.Linq;
using System.Net;

namespace Sockets.Chat.Model
{
    public static class GlobalIPAddress
    {
        public static IPAddress GetGlobalIPAddress()
        {
            string htmlCode;

            using (WebClient client = new WebClient())
                htmlCode = client.DownloadString("http://checkip.dyndns.org");

            return IPAddress.Parse(htmlCode.Split(':')[1]
                .Substring(1)
                .Split('<')
                .First());
        }
    }
}
