using NLog;
using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Clients;
using Sockets.Chat.Model.Data.Messages;
using System;
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
        protected ChatMail ChatMail { get; private set; }
        protected ILogger Logger { get; private set; }
              
        protected object Locker => _lock;

        #endregion

        public TCPChatServer(int port, string serverName, ILogger logger)
        {
            lock (Locker)
            {
                Port = port;
                ServerUser = new ChatUser(Constants.ServerId, serverName);

                Logger = logger;
            }

            ChatMail = new ChatMail(Clients);
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

                var chatClient = ChatClient.Create(currentId, client);
                chatClient.Locker = Locker;

                lock (Locker)
                    Clients.Add(chatClient);

                ChatMail.SendMessage(ChatMessage.Create(MessageCode.ServerName, ServerUser,
                    new ChatUser(currentId), DateTime.Now, ChatMessageText.CreateEmpty()), currentId);

                Logger.Info($"#{currentId} connected!");

                Thread t = new Thread(HandleClients);
                t.Start(currentId);

                currentId += Constants.UserIdIncrementValue;
            }
        }

        #endregion

        #region Protected methods

        protected abstract void OnNewMessage(ChatMessage message);

        #endregion

        #region Private methods

        private void HandleClients(object clientId)
        {
            int id = (int)clientId;
            TcpClient client;

            lock (Locker)
                client = (Clients[id] as ChatClient)?.Client;

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
                lock (Locker)
                    Clients.Remove(id);

                ChatMail.SendMessage(ChatMessage.Create(MessageCode.UserLeave, ServerUser, 
                    null, DateTime.Now, ChatMessageText.Create(id.ToString())));

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

        #endregion  
    }
}
