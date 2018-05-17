using Prism.Commands;
using Prism.Mvvm;
using Sockets.Chat.Client.GUI.Models;
using Sockets.Chat.Client.GUI.Models.Navigation;
using Sockets.Chat.Client.GUI.Models.Resources;
using Sockets.Chat.Client.GUI.Views;
using Sockets.Chat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Chat.Client.GUI.ViewModels
{
    public class ChatPageViewModel : BindableBase
    {
        private TCPChat mChatModel;
        private string mCurrentMessage;

        public ChatPageViewModel()
        {
            InitializeChatCore();

            SendMessageCommand = new DelegateCommand(() => SendMessage());
            DisconnectCommand = new DelegateCommand(() => Disconnect());
        }

        private void Disconnect()
        {
            mChatModel.Disconnect();
            Navigator.Navigate(new StartupPage(),
                new StartupPageViewModel());
        }

        #region Properties

        public string ServerName
        {
            get
            {
                return mChatModel.ServerName;
            }
            set
            {
                mChatModel.ServerName = value;
            }
        }

        public string CurrentMessage
        {
            get
            {
                return mCurrentMessage;
            }
            set
            {
                mCurrentMessage = value;
                RaisePropertyChanged(nameof(CurrentMessage));
            }
        }

        public ObservableCollection<ProxyChatMessage> Messages
        {
            get
            {
                return mChatModel.Messages;
            }
        }

        public ObservableCollection<ChatUser> Users
        {
            get
            {
                return mChatModel.Users;
            }
        }

        #endregion

        #region Private methods

        private void InitializeChatCore()
        {
            mChatModel = ChatService.CurrentChatModel.CreateTCPChat();
            mChatModel.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);

            mChatModel.Connect();
            ResourcesService.Instance.Unloading += (s, e) =>  mChatModel.Disconnect();
        }

        private void SendMessage()
        {
            mChatModel.SendMessage(CurrentMessage);
            CurrentMessage = String.Empty;
        }

        #endregion

        #region Commands

        public DelegateCommand SendMessageCommand { get; }
        public DelegateCommand DisconnectCommand { get; }

        #endregion
    }
}
