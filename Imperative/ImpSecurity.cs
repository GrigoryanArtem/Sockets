// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Security;

namespace Imperative
{
    public class ImpSecurity
    {
        public static void Main(string[] args)
        {
            String option = null;

            if (args.Length > 0)
                option = args[0];
            else
                option = "assert";                  

            //MethodA(option);
            MethodA("deny");
        }

        public static void MethodA(string option) {
            Console.WriteLine("MethodA");
            Console.WriteLine("option - {0}", option);

            IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketPermission permitSocket = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, "127.0.0.1", SocketPermission.AllPorts);

            if (option.Equals("deny"))
                permitSocket.Deny();
            else
                permitSocket.Assert();

            try
            {
                sender.Connect(ipEndPoint);
                Console.WriteLine("Scoket connected to {0}", sender.RemoteEndPoint.ToString());

                byte[] bytes = new byte[1024];
                byte[] msg = Encoding.ASCII.GetBytes("Test message<TheEnd>");

                int bytesSent = sender.Send(msg);

                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket exception!\n {0}", ex.ToString());
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Security exception!\n {0}", ex.ToString());
            }
            finally {
                if (sender.Connected) {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
            Console.WriteLine("Closing MethodA");
        }
    }
}
