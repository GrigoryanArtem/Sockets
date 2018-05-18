using System.Threading;

namespace Sockets.Chat.Model.Clients
{
    public interface ITCPChatClient
    {
        void Connect();
        void Disconnect();
        void SendMessage(ChatMessage message);
    }
}