using Sockets.Chat.Model.Data.Messages;

namespace Sockets.Chat.Model.Data.Clients
{
    public interface IChatClient
    { 
        ChatUser User { get; }
        int Id { get; }
        string Name { get; set; }
        bool IsRegistered { get; }


        void SendMessage(ChatMessage message);
    }
}
