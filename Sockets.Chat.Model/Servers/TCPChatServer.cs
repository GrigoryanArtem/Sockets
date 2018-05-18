using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Chat.Model.Servers
{
    public abstract class TCPChatServer : ITCPChatServer
    {
        #region Members

        private readonly object _lock = new object();

        private MessageHandlersService mMessageHandlers;

        #endregion

        #region Properties

        public ChatUser ServerUser { get; set; }
        public int Port { get; set; }

        protected ChatClients Clients { get; private set; } = new ChatClients();
        protected ILogger Logger { get; private set; }

        #endregion

        public TCPChatServer(int port, string serverName, ILogger logger)
        {
            lock (_lock)
            {
                Port = port;
                ServerUser = new ChatUser(Constants.ServerId, serverName);

                Logger = logger;
            }

            mMessageHandlers = new MessageHandlersService(this);
        }

        #region Public methods

        public void Start()
        {
            int currentId = Constants.FirstUserId;

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, Port);
            ServerSocket.Start();

            Logger.Info($"Server \"{ServerUser.Name}\" started at {GlobalIPAddress.GetGlobalIPAddress()}:{Port}");

            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();

                lock (_lock)
                    Clients.Add(currentId, client);

                SendMessage(ChatMessage.Create(MessageCode.ServerName, ServerUser,
                    new ChatUser(currentId), DateTime.Now, String.Empty), currentId);

                Logger.Info($"#{currentId} connected!");

                Thread t = new Thread(HandleClients);
                t.Start(currentId);

                currentId += Constants.UserIdIncrementValue;
            }
        }

        #endregion

        #region Protected methods

        protected abstract void OnNewMessage(ChatMessage message);

        protected void SendMessage(ChatMessage message, int clientId)
        {
            if(Clients.IsExist(clientId))
                SendMessage(message, Clients[clientId]);
        }

        protected void SendMessage(ChatMessage message, params int[] clientIds)
        {
            SendMessage(message, Clients.GetRegisteredClients(clientIds));
        }

        protected void Broadcast(ChatMessage message)
        {
            SendMessage(message, Clients.RegestredClients);
        }

        #endregion

        #region Private methods

        private void HandleClients(object clientId)
        {
            int id = (int)clientId;
            TcpClient client;

            lock (_lock)
                client = Clients[id];

            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[Constants.MessageSize];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                        break;

                    string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                    HandleMessage(ChatMessage.Parse(data));
                }
            }
            catch(Exception e)
            {
                Logger.Error(e.Message);
                Logger.Error($"Error: #{id} lost connection.");
            }
            finally
            {
                lock (_lock)
                    Clients.Remove(id);

                Broadcast(ChatMessage.Create(MessageCode.UserLeave, ServerUser, null, DateTime.Now, id.ToString()));

                Logger.Info($"#{id} has left.");

                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        private void HandleMessage(ChatMessage message)
        {
            OnNewMessage(message);
            mMessageHandlers.Invoke(message);
        }

        private void SendMessage(ChatMessage message, params TcpClient[] clients)
        {
            SendMessage(message, clients.Select(client => client));
        }

        private void SendMessage(ChatMessage message, IEnumerable<TcpClient> clients)
        {
            byte[] buffer = message.ToByteArray();

            lock (_lock)
            {
                foreach (TcpClient c in clients)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }

            Logger.Trace($"Server sent message: {message}");
        }

        #endregion  
    }
}
