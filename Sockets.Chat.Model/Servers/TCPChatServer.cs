using Sockets.Chat.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sockets.Chat.Model.Servers
{
    public class TCPChatServer
    {
        #region Members

        private readonly object _lock = new object();
        private ILogger mLogger;

        protected Dictionary<int, TcpClient> mClients = new Dictionary<int, TcpClient>();
        protected Dictionary<int, string> mClientsNames = new Dictionary<int, string>();

        #endregion

        #region Properties

        public string ServerName { get; set; }
        public int Port { get; set; }

        #endregion

        public TCPChatServer(int port, string serverName, ILogger logger = null)
        {
            Port = port;
            ServerName = serverName;

            mLogger = logger ?? ConsoleLogger.Instance;
        }

        #region Public methods

        public void Start()
        {
            int currentId = Constants.FirstUserId;

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, Port);
            ServerSocket.Start();

            mLogger.Info($"{DateTime.Now} | Server started at {GlobalIPAddress.GetGlobalIPAddress()}:{Port}");

            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();

                lock (_lock)
                {
                    mClientsNames.Add(currentId, Constants.DefaultName + currentId);
                    mClients.Add(currentId, client);
                }
                
                SendMessage(currentId, ChatMessage.Create(MessageCode.ServerName, 
                    ServerName,DateTime.Now, String.Empty));

                mLogger.Debug($"{DateTime.Now} | {mClientsNames[currentId]} connected!");

                Thread t = new Thread(HandleClients);
                t.Start(currentId);

                currentId += Constants.UserIdIncrementValue;
            }
        }

        #endregion

        private void HandleClients(object clientId)
        {
            int id = (int)clientId;
            TcpClient client;

            lock (_lock)
                client = mClients[id];

            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                        break;

                    string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    var message = ChatMessage.Parse(data);

                    if (message.Sender != mClientsNames[id])
                    {                  
                        mClientsNames[id] = message.Sender;
                        mLogger.Debug($"{DateTime.Now} | {Constants.DefaultName}{id} renamed to {mClientsNames[id]}.");
                    }


                    Broadcast(message);
                    mLogger.Debug($"{DateTime.Now} | {mClientsNames[id]} sent message");
                }
            }
            catch
            {
                mLogger.Error($"{DateTime.Now} | Error: {mClientsNames[id]} lost connection.");
            }
            finally
            {
                var clientName = mClientsNames[id];
                lock (_lock)
                {
                    mClients.Remove(id);
                    mClientsNames.Remove(id);
                }

                mLogger.Debug($"{DateTime.Now} | {clientName} has left.");

                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        #region Protected methods

        protected void SendMessage(int clientId, ChatMessage message)
        {
            byte[] buffer = message.ToByteArray();

            lock (_lock)
            {
                NetworkStream stream = mClients[clientId].GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        protected void Broadcast(ChatMessage message)
        {
            byte[] buffer = message.ToByteArray();

            lock (_lock)
            {
                foreach (TcpClient c in mClients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        #endregion

        #region Private methods


        #endregion  
    }
}
