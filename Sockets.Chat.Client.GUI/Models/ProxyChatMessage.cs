using Sockets.Chat.Model;
using System;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ProxyChatMessage : ChatMessage
    {
        public ProxyChatMessage(ChatMessage message, bool isCurrentUserMessage, MessageType messageType)
            : base(message.Code, message.Sender, message.Recipient, message.Date, message.Message)
        {
            IsCurrentUserMessage = isCurrentUserMessage;
            MessageType = messageType;
        }

        public bool IsCurrentUserMessage { get; private set;}
        public MessageType MessageType { get; private set; }

        public static ProxyChatMessage CurrentUserMessage(ChatMessage message, bool isCurrentUserMessage, MessageType messageType)
        {
            return new ProxyChatMessage(message, isCurrentUserMessage, messageType);
        }

        public static ProxyChatMessage CreateMessageByUser(ChatMessage message, ChatUser user)
        {
            RegexOptions options = RegexOptions.Multiline;

            bool isCurrentUserMessage = (message.Sender.Id == user.Id);
            MessageType messageType = MessageType.Default;

            var match = Regex.Match(message.Message, TCPChatConstants.PrivateMessageRegex, options);
            if (match.Success && ChatUser.Parse(match.Groups["username"].Value).Id == user.Id)
                messageType = MessageType.Private | messageType;

            message.Message = Regex.Replace(message.Message, TCPChatConstants.PrivateMessageRegex, String.Empty);

            foreach (Match m in Regex.Matches(message.Message, TCPChatConstants.SelectedMessageRegex, options))
            {
                var curentUser = ChatUser.Parse(m.Groups["username"].Value);

                if (messageType < MessageType.Selected && curentUser.Id == user.Id)
                    messageType = MessageType.Selected | messageType;

                message.Message = Regex.Replace(message.Message, m.Value, curentUser.Name);           
            }

            return CurrentUserMessage(message, isCurrentUserMessage, messageType);
        }
    }
}
