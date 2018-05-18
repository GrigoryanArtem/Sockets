using Prism.Commands;
using Prism.Mvvm;
using Sockets.Chat.Client.GUI.Models;
using Sockets.Chat.Client.GUI.Models.Navigation;
using Sockets.Chat.Client.GUI.Views;
using Sockets.Chat.Model.Clients;
using System;

namespace Sockets.Chat.Client.GUI.ViewModels
{
    public class StartupPageViewModel : BindableBase
    {
        private ChatModel mModel;
        private string mErrorMessage;

        public StartupPageViewModel()
        {
            mModel = ChatService.CurrentChatModel;
            mModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            StartChatCommand = new DelegateCommand(() => StartChat());
            ClearErrorCommand = new DelegateCommand(() => ErrorMessage = String.Empty);
        }

        #region Properties

        public string UserName
        {
            get
            {
                return mModel.UserName;
            }
            set
            {
                mModel.UserName = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return mModel.IPAddress;
            }
            set
            {
                mModel.IPAddress = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return mErrorMessage;
            }
            set
            {
                mErrorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion

        #region Private methods

        private void StartChat()
        {
            try
            {
                mModel.Save();

                var chatPage = new ChatPage();
                Navigator.Navigate(chatPage, new ChatPageViewModel());
            }
            catch(ServerUnavailableException)
            {
                ErrorMessage = "The server is currently unavailable, please try again later.";
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message; 
            }
        }

        #endregion

        #region Commands

        public DelegateCommand StartChatCommand { get; }
        public DelegateCommand ClearErrorCommand { get; }

        #endregion
    }
}
