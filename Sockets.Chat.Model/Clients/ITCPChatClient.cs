using System.Threading;

namespace Sockets.Chat.Model.Clients
{
    public interface ITCPChatClient
    {
        void Connect(ParameterizedThreadStart receiveData);
        void Disconnect();
        void SendMessage(ChatMessage message);
    }
}