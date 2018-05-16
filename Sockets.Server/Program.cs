using Sockets.Model;
using System;
using System.Linq;
using System.Net;

namespace Sockets.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {          
                TCPServer server = new TCPServer(args[0], Convert.ToInt32(args[1]));
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in server occured!\n {e.Message}");
            }
        }
    }
}
