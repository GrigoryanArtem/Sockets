using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ChatModel : BindableBase
    {
        #region Members

        private string mUserName;
        private string mIpAddress;

        #endregion

        public ChatModel()
        {
            LoadProperties();
        }

        #region Properties

        public string UserName
        {
            get
            {
                return mUserName;
            }
            set
            {
                mUserName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string IPAddress
        {
            get
            {
                return mIpAddress;
            }
            set
            {
                mIpAddress = value;
                RaisePropertyChanged(nameof(IPAddress));
            }
        }

        #endregion

        #region Public methods

        public void Save()
        {
            Properties.Settings.Default.UserName = UserName;
            Properties.Settings.Default.IpAddress = IPAddress;

            Properties.Settings.Default.Save();
        }

        public TCPChat CreateTCPChat()
        {
            return new TCPChat(UserName, IPAddress);
        }

        #endregion

        #region Private methods

        private void LoadProperties()
        {
            UserName = Properties.Settings.Default.UserName;
            IPAddress = Properties.Settings.Default.IpAddress;
        }

        #endregion
    }
}
