// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using NLog;
using Sockets.Chat.Model;
using Sockets.Chat.Model.Servers;
using System;

namespace Sockets.Chat.Server.CUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                int port = Convert.ToInt32(args[1]);
                string serverName = args[0];

                ITCPChatServer server = new DefaultTCPChatServer(port, serverName, logger);
                server.Start();
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
