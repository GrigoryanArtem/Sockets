using Sockets.Chat.Model;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ProxyChatMessage : ChatMessage
    {
        public ProxyChatMessage(ChatMessage message, bool isCurrentUserMessage)
            : base(message.Code, message.Sender, null, message.Date, message.Message)
        {
            IsCurrentUserMessage = isCurrentUserMessage;
        }

        public bool IsCurrentUserMessage { get; private set;}

        public static ProxyChatMessage CurrentUserMessage(ChatMessage message, bool isCurrentUserMessage)
        {
            return new ProxyChatMessage(message, isCurrentUserMessage);
        }
    }
}
