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

namespace Sockets.Chat.Client.GUI.Models
{
    public class TCPChat : BindableBase
    {
        #region Members

        private TCPChatCore mChatCore;
        private MessageHandlersService mMessageHandlers;

        private ChatUser mUser;
        private string mServerName;

        #endregion

        public TCPChat(string username, string ipAddress)
        {
            mUser = new ChatUser(0, username);
            mChatCore = new TCPChatCore(ipAddress, Properties.Settings.Default.Port);

            mMessageHandlers = new MessageHandlersService(this);
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

        public ObservableCollection<ProxyChatMessage> Messages { get; private set; } 
            = new ObservableCollection<ProxyChatMessage>();

        public ObservableCollection<ChatUser> Users { get; private set; }
            = new ObservableCollection<ChatUser>();
        public SnackbarMessageQueue NotificationQueue { get; private set; } 
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        #endregion

        #region Public methods

        public void Connect()
        {
            mChatCore.Connect(client => ReceiveData((TcpClient)client));
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
            RegexOptions options = RegexOptions.Multiline;
            ChatUser recipient = null;

            var match = Regex.Match(message, TCPChatConstants.PrivateMessageRegex, options);
            if (match.Success)
                recipient = ChatUser.Parse(match.Groups["username"].Value);

            mChatCore.SendMessage(ChatMessage.Create(MessageCode.Message, 
                mUser, recipient, DateTime.Now, message));
        }

        private void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[TCPChatConstants.DefaultMessageSize];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                var messageData = Encoding.UTF8.GetString(receivedBytes, 0, byte_count);
                var message = ChatMessage.Parse(messageData);

                mMessageHandlers.Invoke(message);
            }
        }

        #endregion

        #region Handlers

        [MessageHandler(MessageCode.Message)]
        private void OnNewMessage(ChatMessage message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(
                    ProxyChatMessage.CreateMessageByUser(message, mUser)));
        }

        [MessageHandler(MessageCode.ServerName)]
        private void OnRegistration(ChatMessage message)
        {
            ServerName = message.Sender.Name;

            mUser.Id = message.Recipient.Id;
            SendMessage(ChatMessage.Create(MessageCode.Registration, mUser, new ChatUser(0, mServerName), DateTime.Now, String.Empty));
        }

        [MessageHandler(MessageCode.ServerUsers)]
        private void OnServerUsers(ChatMessage message)
        {
            if (String.IsNullOrEmpty(message.Message) || String.IsNullOrWhiteSpace(message.Message))
                return;

            var users = message.Message
            .Split(' ')
            .Select(user => ChatUser.Parse(user));

            Application.Current.Dispatcher.Invoke(() => Users.AddRange(users));          
        }

        [MessageHandler(MessageCode.NewUser)]
        private void OnNewUser(ChatMessage message)
        {
            var user = ChatUser.Parse(message.Message);

            Application.Current.Dispatcher.Invoke(() => Users.Add(user));

            if(user.Id != mUser.Id)
                Task.Factory.StartNew(() => NotificationQueue.Enqueue($"New user: {user.Name}", "OK", (username) => { }, user.Name));
        }

        [MessageHandler(MessageCode.UserLeave)]
        private void OnUserLeave(ChatMessage message)
        {
            var user = Users.FirstOrDefault(u => u.Id.ToString() == message.Message);

            if (user != null)
            {
                Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
                Task.Factory.StartNew(() => NotificationQueue.Enqueue($"User {user.Name} leave", "OK", (username) => { }, user.Name));
            }
        }

        #endregion
    }
}
