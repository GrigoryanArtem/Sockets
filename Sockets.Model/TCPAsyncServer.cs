using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Model
{
    public class TCPAsyncServer
    {
        #region Members

        private byte[] buffer = new byte[Constants.MessageSize];
        private ManualResetEvent socketEvent = new ManualResetEvent(false);

        #endregion

        #region Properties

        public static string IpAddress { get; set; }
        public static int Port { get; set; }
        public static int BackLog { get; set; } = Constants.DefaultBackLog;

        #endregion

        public TCPAsyncServer() { }

        public TCPAsyncServer(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public void Start()
        {
            Console.WriteLine("Main Thread ID: " + AppDomain.GetCurrentThreadId());

            byte[] bytes = new byte[Constants.MessageSize];

            IPHostEntry ipHost = Dns.Resolve(IpAddress);
            IPAddress ipAddr = ipHost.AddressList.First();

            IPEndPoint localEnd = new IPEndPoint(ipAddr, Port);

            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sListener.Bind(localEnd);

            sListener.Listen(10);

            Console.WriteLine("Waiting for a connection...");

            AsyncCallback aCallback = new AsyncCallback(AcceptCallback);

            sListener.BeginAccept(aCallback, sListener);
            socketEvent.WaitOne();
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Console.WriteLine("AcceptCallback Thread ID:" + AppDomain.GetCurrentThreadId());

            Socket listener = (Socket)result.AsyncState;
            Socket handler = listener.EndAccept(result);

            handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), handler);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Console.WriteLine("ReceiveCallback Thread ID:" + AppDomain.GetCurrentThreadId());

            string content = String.Empty;

            Socket handler = (Socket)result.AsyncState;

            int bytesRead = handler.EndReceive(result);

            if (bytesRead > 0)
            {
                content += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (content.Contains("<TheEnd>"))
                {
                    Console.WriteLine("Read {0} bytes from socket.\nData: {1}", content.Length, content);

                    byte[] byteData = Encoding.ASCII.GetBytes(content);
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                }
                else
                {
                    handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), handler);
                }
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            Console.WriteLine($"SendCallback Thread ID: {AppDomain.GetCurrentThreadId()}");

            Socket handler = (Socket)result.AsyncState;
            int bytesSent = handler.EndSend(result);

            Console.WriteLine($"Sent {bytesSent} bytes to Client.");

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            socketEvent.Set();
        }
    }
}
