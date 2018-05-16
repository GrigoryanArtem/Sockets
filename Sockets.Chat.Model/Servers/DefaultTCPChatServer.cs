using Sockets.Chat.Model.Loggers;
using System;

namespace Sockets.Chat.Model.Servers
{
    public class DefaultTCPChatServer : TCPChatServer
    {
        public DefaultTCPChatServer(int port, string serverName, ILogger logger = null)
            : base(port, serverName, logger) { }

        protected override void OnNewMessage() { }

        #region Handlers

        [MessageHandler(MessageCode.Registration)]
        private void OnRegistration(ChatMessage message)
        {
            Logger.Debug($"Registration new user: {message.Sender.Name}.");

            Clients.Registration(message.Sender.Id, message.Sender.Name);
            
            Broadcast(ChatMessage.Create(MessageCode.NewUser, ServerUser, null, DateTime.Now, String.Empty));
            SendMessage(ChatMessage.Create(MessageCode.ServerUsers, ServerUser, message.Sender,
                DateTime.Now, String.Join(" ", Clients.RegestredUsers)));
        }

        [MessageHandler(MessageCode.Message)]
        private void OnNewMessage(ChatMessage message)
        {
            Logger.Debug($"{message.Sender} sent message. {message}");

            if (message.Recipient is null)
                Broadcast(message);
            else
                SendMessage(message, message.Recipient.Id);
        }

        #endregion
    }
}
