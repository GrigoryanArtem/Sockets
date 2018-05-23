using Sockets.Chat.Model.Data.Messages;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Sockets.Chat.Model.Data.Clients
{
    public class ChatRoomClient : DefaultChatClient
    {
        public List<ChatClient> Clients { get; } = new List<ChatClient>();

        public ChatRoomClient(int id, string name = null)
            : base(id, name) { }

        public override void SendMessage(ChatMessage message)
        {
            byte[] buffer = message.ToByteArray();

            lock (Locker)
            {
                Clients.ForEach(client => {
                    NetworkStream stream = client.Client.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }); 
            }
        }

        public static ChatRoomClient Create(int id, string name = null, params ChatClient[] clients)
        {
            ChatRoomClient result = new ChatRoomClient(id, name);
            result.Clients.AddRange(clients);

            return result;
        }
    }
}
