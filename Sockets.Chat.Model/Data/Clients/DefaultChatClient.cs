using Sockets.Chat.Model.Data.Messages;
using System;

namespace Sockets.Chat.Model.Data.Clients
{
    public abstract class DefaultChatClient : IChatClient
    {
        #region Properties

        public object Locker { get; set; } = new object();
        public ChatUser User { get; private set; }

        public int Id => User.Id;
        public string Name
        {
            get => User.Name;
            set => User.Name = value;
        }

        public bool IsRegistered => !(String.IsNullOrEmpty(Name) ||
            String.IsNullOrWhiteSpace(Name));

        #endregion

        public DefaultChatClient(int id, string name = null)
        {
            User = new ChatUser(id, name);
        }

        public abstract void SendMessage(ChatMessage message);
    }
}
