// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Net;
using System.Net.Sockets;

namespace PortListener
{
    public class PortListener
    {
        static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");

            for (int i = 1; i < 10; i++) {
                Console.WriteLine("Checking port {0}", i);

                try
                {
                    IPEndPoint endPoint = new IPEndPoint(address, i);
                    Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sSocket.Connect(endPoint);

                    Console.WriteLine("Port {0} is listening", i);
                }
                catch (SocketException ignored) {
                    if (ignored.ErrorCode != 10061)
                        Console.WriteLine(ignored.Message);
                }
            }
        }
    }
}
