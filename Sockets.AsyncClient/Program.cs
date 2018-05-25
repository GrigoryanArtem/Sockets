// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Sockets.Model;
using System;

namespace Sockets.AsyncClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPAsyncClient client = new TCPAsyncClient(args[0], Convert.ToInt32(args[1]));
            client.Start();
        }
    }
}
