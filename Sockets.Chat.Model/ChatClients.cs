using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Chat.Model
{
    public class ChatClients
    {
        #region Members

        private Dictionary<int, TcpClient> mClients = new Dictionary<int, TcpClient>();
        private Dictionary<int, string> mClientsNames = new Dictionary<int, string>();
        private HashSet<int> IsRegisteredUsers = new HashSet<int>();

        #endregion


    }
}
