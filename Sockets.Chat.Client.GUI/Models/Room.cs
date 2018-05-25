using Prism.Mvvm;
using Sockets.Chat.Model.Data;
using System.Collections.ObjectModel;

namespace Sockets.Chat.Client.GUI.Models
{
    public class Room : BindableBase
    {
        #region Members

        private string mName;

        #endregion

        public Room(string name, ChatUser recipient = null)
        {
            Name = name;
            Recipient = recipient;
        }

        public ObservableCollection<ChatUser> Users { get; } 
            = new ObservableCollection<ChatUser>();
        public ObservableCollection<ProxyChatMessage> Messages { get; }
            = new ObservableCollection<ProxyChatMessage>();
        public ChatUser Recipient { get; set; }

        public string Name
        {
            get => mName;
            set
            {
                mName = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

    }
}
