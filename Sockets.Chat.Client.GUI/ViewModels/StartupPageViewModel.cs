using Prism.Commands;
using Prism.Mvvm;
using Sockets.Chat.Client.GUI.Models;
using Sockets.Chat.Client.GUI.Models.Navigation;
using Sockets.Chat.Client.GUI.Views;

namespace Sockets.Chat.Client.GUI.ViewModels
{
    public class StartupPageViewModel : BindableBase
    {
        private ChatModel mModel;

        public StartupPageViewModel()
        {
            mModel = ChatService.CurrentChatModel;
            mModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            StartChatCommand = new DelegateCommand(() => StartChat());
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

        #endregion

        #region Private methods

        private void StartChat()
        {
            mModel.Save();

            var chatPage = new ChatPage();
            Navigator.Navigate(chatPage, new ChatPageViewModel());
        }

        #endregion

        #region Commands

        public DelegateCommand StartChatCommand { get; }

        #endregion
    }
}
