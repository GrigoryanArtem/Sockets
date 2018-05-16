using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Security;

namespace Declarative
{
    public class DecSecutity
    {
        static void Main(string[] args)
        {
            LegalMethod();
            IllegalMethod();
        }

        [SocketPermission(System.Security.Permissions.SecurityAction.Assert, Access = "Connect", 
            Host = "127.0.0.1", Port = "All", Transport = "Tcp")]
        public static void LegalMethod()
        {
            Console.WriteLine("LegalMethod");

            IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(ipEndPoint);
                Console.WriteLine("Scoket connected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket exception!\n {0}", ex.ToString());
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Security exception!\n {0}", ex.ToString());
            }
            finally
            {
                if (sender.Connected)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
        }

        [SocketPermission(System.Security.Permissions.SecurityAction.Deny, Access = "Connect",
    Host = "127.0.0.1", Port = "All", Transport = "Tcp")]
        public static void IllegalMethod()
        {
            Console.WriteLine("LegalMethod");

            IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(ipEndPoint);
                Console.WriteLine("Scoket connected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket exception!\n {0}", ex.ToString());
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Security exception!\n {0}", ex.ToString());
            }
            finally
            {
                if (sender.Connected)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
        }
    }
}
