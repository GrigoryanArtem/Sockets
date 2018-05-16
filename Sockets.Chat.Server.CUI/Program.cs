using Sockets.Chat.Model;
using Sockets.Chat.Model.Loggers;
using Sockets.Chat.Model.Servers;
using System;

namespace Sockets.Chat.Server.CUI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ITCPChatServer server = new DefaultTCPChatServer(11000, "Server");
                server.Start();
            }
            catch(Exception e)
            {
                ConsoleLogger.Instance.Error(e.Message);
            }
        }
    }
}
