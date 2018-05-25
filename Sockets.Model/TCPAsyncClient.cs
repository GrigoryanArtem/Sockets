// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Model
{
    public class TCPAsyncClient
    {
        #region Members

        private string theResponce = String.Empty;
        private byte[] buffer = new byte[Constants.MessageSize];

        private ManualResetEvent ConnectDone = new ManualResetEvent(false);
        private ManualResetEvent SendDone = new ManualResetEvent(false);
        private ManualResetEvent ReceiveDone = new ManualResetEvent(false);

        #endregion

        #region Properties

        public static string IpAddress { get; set; }
        public static int Port { get; set; }

        #endregion

        public TCPAsyncClient() { }

        public TCPAsyncClient (string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public void Start()
        {
            try
            {
                Thread thr = Thread.CurrentThread;
                Console.WriteLine("Main Thread State: " + thr.ThreadState);

                IPHostEntry ipHost = Dns.Resolve(IpAddress);
                IPAddress ipAddr = ipHost.AddressList.First();
                IPEndPoint endPoint = new IPEndPoint(ipAddr, Port);

                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sClient.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), sClient);
                ConnectDone.WaitOne();

                string data = "The coolest test message!";
                for (int i = 0; i < 72; i++)
                {
                    data += i.ToString() + ";" + (new string('=', i));
                }

                byte[] byteData = Encoding.ASCII.GetBytes(data + "<TheEnd>");

                sClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), sClient);

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Trying to sleep {0} time", i);
                    Thread.Sleep(10);
                }

                SendDone.WaitOne();

                sClient.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                ReceiveDone.WaitOne();

                Console.WriteLine("Response received {0}", theResponce);

                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("ConnectCallBack Thread State: " + thr.ThreadState);

            Socket sClient = (Socket)ar.AsyncState;

            sClient.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}", sClient.RemoteEndPoint.ToString());

            ConnectDone.Set();
        }

        private void SendCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("SendCallback Thread State: " + thr.ThreadState);

            Socket sClient = (Socket)ar.AsyncState;

            int bytesSent = sClient.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            SendDone.Set();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("ReceiveCallback Thread State: " + thr.ThreadState);

            Socket sClient = (Socket)ar.AsyncState;

            int bytesRead = sClient.EndReceive(ar);

            if (bytesRead > 0)
            {
                theResponce += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                sClient.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sClient);
            }
            else
            {
                ReceiveDone.Set();
            }
        }
    }
}
