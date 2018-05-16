using Sockets.Model;
using System;

namespace Sockets.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TCPClient client = new TCPClient(args[0], Convert.ToInt32(args[1]));
                client.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in client occured: {e.Message}");
            }
        }
    }
}
