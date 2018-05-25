using NLog;
using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Messages;
using System;
using System.Linq;

namespace Sockets.Chat.Model.Servers
{
    public class DefaultTCPChatServer : TCPChatServer
    {
        public DefaultTCPChatServer(int port, string serverName, ILogger logger)
            : base(port, serverName, logger) { }

        protected override void OnNewMessage(ChatMessage message)
        {
            Logger.Trace($"{message.Sender} sent message. {message}");
        }

        #region Handlers

        [MessageHandler(MessageCode.Registration)]
        private void OnRegistration(ChatMessage message)
        {
            Logger.Debug($"Registration new user: {message.Sender.Name}.");

            Clients[message.Sender.Id].Name = message.Sender.Name;


            ChatMail.SendMessage(ChatMessage.Create(MessageCode.NewUser, ServerUser, null, DateTime.Now, ChatMessageText.Create(message.Sender.ToString())));
            ChatMail.SendMessage(ChatMessage.Create(MessageCode.ServerUsers, ServerUser, message.Sender,
                DateTime.Now, ChatMessageText.Create(String.Join(" ", Clients.RegestredUsers.Where(user => user.Id != message.Sender.Id).Select(user => user.User)))), message.Sender.Id);
        }

        [MessageHandler(MessageCode.PublicMessage)]
        private void OnNewUserPublicMessage(ChatMessage message)
        {            
                ChatMail.SendMessage(message);            
        }

        [MessageHandler(MessageCode.Message)]
        private void OnNewUserMessage(ChatMessage message)
        {
            if (message.Recipient is null)
                throw new ArgumentException(nameof(message));

            if (!message.Sender.Equals(message.Recipient))
                ChatMail.SendMessage(ChatMessage.Create(MessageCode.RepeatMessage, message.Sender,
                    message.Recipient, message.Date, message.Message), message.Sender.Id);

            ChatMail.SendMessage(message, message.Recipient.Id);
        }

        #endregion
    }
}
