using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Sockets.Chat.Client.GUI.Models;
using Sockets.Chat.Client.GUI.Models.Navigation;
using Sockets.Chat.Client.GUI.Models.Resources;
using Sockets.Chat.Client.GUI.Views;
using Sockets.Chat.Model.Data;
using System;

namespace Sockets.Chat.Client.GUI.ViewModels
{
    public class ChatPageViewModel : BindableBase
    {
        private TCPChat mChatModel;
        private string mCurrentMessage;

        private bool mIsUsersOpen;

        public ChatPageViewModel()
        {
            InitializeChatCore();

            SendMessageCommand = new DelegateCommand(() => SendMessage());
            DisconnectCommand = new DelegateCommand(() => Disconnect());
            OpenUsersCommand = new DelegateCommand(() => IsUsersOpen = true);

            MentionUserCommand = new DelegateCommand<ChatUser>(user => MentionUser(user));
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

        public bool IsUsersOpen
        {
            get
            {
                return mIsUsersOpen;
            }
            set
            {
                mIsUsersOpen = value;
                RaisePropertyChanged(nameof(IsUsersOpen));
            }
        }

        public SnackbarMessageQueue NotificationQueue
        {
            get
            {
                return mChatModel.NotificationQueue;
            }
        }

        public RoomsManager RoomsManager => mChatModel.RoomsManager;

        public int NumberOfUnreadMessages => mChatModel.NumberOfUnreadMessages;

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

        private void MentionUser(ChatUser user)
        {
            CurrentMessage += $"@{user}";
            IsUsersOpen = false;
        }

        #endregion

        #region Commands

        public DelegateCommand SendMessageCommand { get; }
        public DelegateCommand DisconnectCommand { get; }
        public DelegateCommand OpenUsersCommand { get; }

        public DelegateCommand<ChatUser> MentionUserCommand { get; }
        #endregion
    }
}
