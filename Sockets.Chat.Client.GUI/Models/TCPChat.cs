using Prism.Mvvm;
using Sockets.Chat.Model;
using Sockets.Chat.Model.Clients;
using Sockets.Chat.Model.Servers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using System.Text.RegularExpressions;
using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Messages;

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

        #endregion

        #region Handlers


        [MessageHandler(MessageCode.PublicMessage)]
        private void OnNewPublicMessage(ChatMessage message)
        {
            Application.Current.Dispatcher.Invoke(() => {
                mPublicRoom.Messages.Add(ProxyChatMessage.CreateMessageByUser(message, mUser));
            });
        }

        [MessageHandler(MessageCode.Message)]
        private void OnNewMessage(ChatMessage message)
        {
            Application.Current.Dispatcher.Invoke(() => {
                RoomsManager.Rooms
                .FirstOrDefault(r => (r.Recipient != null && r.Recipient.Equals(message.Sender)))
                ?.Messages.Add(ProxyChatMessage.CreateMessageByUser(message, mUser));
            });
        }

        [MessageHandler(MessageCode.RepeatMessage)]
        private void OnRepeatMEssage(ChatMessage message)
        {
            Application.Current.Dispatcher.Invoke(() => {
                RoomsManager.Rooms
                .FirstOrDefault(r => (r.Recipient != null && r.Recipient.Equals(message.Recipient)))
                ?.Messages.Add(ProxyChatMessage.CreateMessageByUser(message, mUser));
            });
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
            var rooms = users.Select(u => new Room(u.Name, u));

            Application.Current.Dispatcher.Invoke(() => {
                mPublicRoom.Users.AddRange(users);
                RoomsManager.Rooms.AddRange(rooms);
            });
        }

        [MessageHandler(MessageCode.NewUser)]
        private void OnNewUser(ChatMessage message)
        {
            var user = ChatUser.Parse(message.Message.Text);

            Application.Current.Dispatcher.Invoke(() => {
                mPublicRoom.Users.Add(user);
                RoomsManager.Rooms.Add(new Room(user.Name, user));
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
