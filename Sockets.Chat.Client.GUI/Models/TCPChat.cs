using Prism.Mvvm;
using Sockets.Chat.Model;
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

namespace Sockets.Chat.Client.GUI.Models
{
    public class TCPChat : BindableBase
    {
        #region Members

        private TCPChatCore mChatCore;

        private string mUserName;
        private string mServerName;

        #endregion

        public TCPChat(string username, string ipAddress)
        {
            mUserName = username;
            mChatCore = new TCPChatCore(ipAddress, Properties.Settings.Default.Port);
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

        public ObservableCollection<string> Users { get; private set; }
            = new ObservableCollection<string>();

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
            SendMessage(ChatMessage.Create(MessageCode.Message, 
                mUserName, DateTime.Now, message));
        }

        private void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[TCPChatConstants.DefaultMessageSize];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                var message = ChatMessage.Parse(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));

                if (message.Code == MessageCode.Message)
                    Application.Current.Dispatcher.Invoke(() => Messages.Add(
                        ProxyChatMessage.CurrentUserMessage(message, message.Sender == mUserName)));
                else if (message.Code == MessageCode.ServerName)
                    ServerName = message.Sender;
                else if (message.Code == MessageCode.NewUser)
                    Application.Current.Dispatcher.Invoke(() => Users.Add(message.Message));
                else if (message.Code == MessageCode.UserLeave)
                    Application.Current.Dispatcher.Invoke(() => Users.Remove(message.Message));
            }
        }

        #endregion
    }
}
