using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using Sockets.Chat.Model;
using Sockets.Chat.Model.Clients;
using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Messages;
using Sockets.Chat.Model.Servers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sockets.Chat.Client.GUI.Models
{
    public class TCPChat : BindableBase
    {
        #region Members

        private ITCPChatClient mChatCore;

        private Room mPublicRoom;
        private ChatUser mUser;
        private string mServerName;

        #endregion

        public TCPChat(string username, string ipAddress)
        {
            mUser = new ChatUser(0, username);
            mChatCore = new TCPChatCore(ipAddress, Properties.Settings.Default.Port, this);

            mPublicRoom = new Room("Public");

            RoomsManager.Rooms.Add(mPublicRoom);
            RoomsManager.CurrentRoom = mPublicRoom;

            RoomsManager.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);
        }

        #region Properties

        public string ServerName
        {
            get
            {
                return mServerName;
            }
            set
            {
                mServerName = value;
                RaisePropertyChanged(nameof(ServerName));
            }
        }

        public RoomsManager RoomsManager { get; private set; }
           = new RoomsManager();

        public SnackbarMessageQueue NotificationQueue { get; private set; } 
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        public int NumberOfUnreadMessages => RoomsManager.NumberOfUnreadMessages;

        #endregion

        #region Public methods

        public void Connect()
        {
            mChatCore.Connect();
        }

        public void Disconnect()
        {
            mChatCore.Disconnect();
        }

        public void SendMessage(ChatMessage message)
        {
            mChatCore.SendMessage(message);
        }

        public void SendMessage(string message)
        {
            ChatMessageText messageText = ChatMessageText.Create(message);

            mChatCore.SendMessage(ChatMessage.Create(GetMessageCodeByRecipient(RoomsManager.CurrentRoom.Recipient), 
                mUser, RoomsManager.CurrentRoom.Recipient, DateTime.Now, messageText));
        }

        private MessageCode GetMessageCodeByRecipient(ChatUser recipient)
        {
            return recipient is null ? MessageCode.PublicMessage : MessageCode.Message;
        }

        private void AddMessage(ChatMessage message, Room room)
        {
            RoomsManager.AddMessage(ProxyChatMessage.CreateMessageByUser(message, mUser), room);
        }

        #endregion

        #region Handlers


        [MessageHandler(MessageCode.PublicMessage)]
        private void OnNewPublicMessage(ChatMessage message)
        {
            AddMessage(message, mPublicRoom);
        }

        [MessageHandler(MessageCode.Message)]
        private void OnNewMessage(ChatMessage message)
        {
            AddMessage(message, RoomsManager.Rooms.FirstOrDefault(
                r => (r.Recipient != null && r.Recipient.Equals(message.Sender))));
        }

        [MessageHandler(MessageCode.RepeatMessage)]
        private void OnRepeatMEssage(ChatMessage message)
        {
            AddMessage(message, RoomsManager.Rooms.FirstOrDefault(
                r => (r.Recipient != null && r.Recipient.Equals(message.Recipient))));
        }

        [MessageHandler(MessageCode.ServerName)]
        private void OnRegistration(ChatMessage message)
        {
            ServerName = message.Sender.Name;

            mUser = new ChatUser(message.Recipient.Id, mUser.Name);
            SendMessage(ChatMessage.Create(MessageCode.Registration, mUser, new ChatUser(0, mServerName), DateTime.Now, ChatMessageText.CreateEmpty()));
        }

        [MessageHandler(MessageCode.ServerUsers)]
        private void OnServerUsers(ChatMessage message)
        {
            if (String.IsNullOrEmpty(message.Message.Text) || String.IsNullOrWhiteSpace(message.Message.Text))
                return;

            var users = message.Message.Text
            .Split(' ')
            .Select(user => ChatUser.Parse(user));
            var rooms = users.Select(u => Room.CreateRoomByUser(u, u, mUser));

            Application.Current.Dispatcher.Invoke(() => {
                mPublicRoom.Users.AddRange(users);
                RoomsManager.Rooms.AddRange(rooms);
            });
        }

        [MessageHandler(MessageCode.NewUser)]
        private void OnNewUser(ChatMessage message)
        {
            var user = ChatUser.Parse(message.Message.Text);
            var room = (user.Equals(mUser)) ? Room.CreateSelfRoom(user) : 
                Room.CreateRoomByUser(user, user, mUser);

            Application.Current.Dispatcher.Invoke(() => {
                mPublicRoom.Users.Add(user);
                RoomsManager.Rooms.Add(room);
            });

            if (user.Id != mUser.Id)
                Task.Factory.StartNew(() => NotificationQueue.Enqueue($"A new user {user.Name} has joined", "OK", (username) => { }, user.Name));
        }

        [MessageHandler(MessageCode.UserLeave)]
        private void OnUserLeave(ChatMessage message)
        {
            var user = mPublicRoom.Users.FirstOrDefault(u => u.Id.ToString() == message.Message.Text);
            var room = RoomsManager.Rooms.FirstOrDefault(r => r.Recipient != null && r.Recipient.Equals(user));

            if (user != null)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    mPublicRoom.Users.Remove(user);
                    RoomsManager.Rooms.Remove(room);
                });

                Task.Factory.StartNew(() => NotificationQueue.Enqueue($"User {user.Name} left the chat", "OK", (username) => { }, user.Name));
            }
        }

        #endregion
    }
}
