// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets.Model
{
    public class TCPClient
    {
        #region Properties

        public static string IpAddress { get; set; }
        public static int Port { get; set; }

        #endregion

        public TCPClient() { }

        public TCPClient(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public void Start()
        {
            byte[] bytes = new byte[Constants.MessageSize];
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                IPHostEntry ipHost = Dns.Resolve(IpAddress);
                IPAddress ipAddr = ipHost.AddressList.First();
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Port);

                sender.Connect(ipEndPoint);

                Console.WriteLine($"Socket conencted to {sender.RemoteEndPoint.ToString()}");

                string theMessage = "The coolest message!";
                byte[] msg = Encoding.ASCII.GetBytes(theMessage + "<TheEnd>");

                int bytesSent = sender.Send(msg);
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("The Server says: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sender.Connected)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
        }
    }
}
