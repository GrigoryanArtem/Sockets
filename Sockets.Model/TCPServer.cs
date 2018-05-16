using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets.Model
{
    public class TCPServer
    {
        #region Properties

        public static string IpAddress { get; set; }
        public static int Port { get; set; }
        public static int BackLog { get; set; } = Constants.DefaultBackLog;

        #endregion

        public TCPServer() { }

        public TCPServer(string ipAddress, int port)
        { 
            IpAddress = ipAddress;
            Port = port;
        }

        public void Start()
        {
            IPHostEntry ipHost = Dns.Resolve(IpAddress);
            IPAddress ipAddr = ipHost.AddressList.First();
            IPEndPoint iPEndPoint = new IPEndPoint(ipAddr, Port);

            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sListener.Bind(iPEndPoint);
            sListener.Listen(BackLog);

            while (true)
            {
                Console.WriteLine($"Waiting for a connection on port {iPEndPoint}.");

                Socket handler = sListener.Accept();
                string data = String.Empty;

                while (true)
                {
                    byte[] bytes = new byte[Constants.MessageSize];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    if (data.Contains("<TheEnd>"))
                        break;
                }

                Console.WriteLine($"Text Received : {data}");
                string theReply = $"Thank you for those {data.Length} characters.";
                byte[] msg = Encoding.ASCII.GetBytes(theReply);

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}
