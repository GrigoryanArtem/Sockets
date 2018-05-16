using Sockets.Chat.Model;
using System;

namespace Sockets.Chat.Server.CUI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TCPChatServer server = new TCPChatServer(11000, "WPF THE BEST!");
                server.Start();
            }
            catch(Exception e)
            {
                ConsoleLogger.Instance.Error(e.Message);
            }
        }
    }
}
