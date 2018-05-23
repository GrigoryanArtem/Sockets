using Sockets.Chat.Model;
using Sockets.Chat.Model.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sockets.Chat.Client.CUI
{
    class Program
    {
        private static string Name;

        static void Main(string[] args)
        {
            Console.Write("Enter name: ");
            Name = Console.ReadLine();

            try
            {
                IPAddress ip = IPAddress.Parse(args[0]);
                int port = 11000;
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                Console.WriteLine("Connected to server!");
                NetworkStream ns = client.GetStream();
                Thread thread = new Thread(o => ReceiveData((TcpClient)o));

                thread.Start(client);

                string s;
                while (!string.IsNullOrEmpty((s = Console.ReadLine())))
                {
                    byte[] buffer = Encoding.ASCII.GetBytes($"{{0, \"{Name}\", {DateTime.Now}, \"{s}\"}}");
                    ns.Write(buffer, 0, buffer.Length);
                }

                client.Client.Shutdown(SocketShutdown.Send);
                thread.Join();
                ns.Close();
                client.Close();
                Console.WriteLine("Disconnected from server!");
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                var message = ChatMessage.Parse(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine($"{message.Date.ToShortTimeString()} {message.Sender}: {message.Message}");

                Console.ResetColor();
            }
        }
    }
}
