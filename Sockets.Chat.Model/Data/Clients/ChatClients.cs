using System;
using System.Collections.Generic;
using System.Linq;

namespace Sockets.Chat.Model.Data.Clients
{
    public class ChatClients
    {
        #region Members
        
        private Dictionary<int, ChatClient> mClients = new Dictionary<int, ChatClient>();
        private Dictionary<int, ChatRoomClient> mRooms = new Dictionary<int, ChatRoomClient>();

        #endregion

        #region Properties

        public IEnumerable<ChatClient> Users => mClients.Values;
        public IEnumerable<ChatClient> RegestredUsers => Users.Where(client => client.IsRegistered);
        public IEnumerable<ChatRoomClient> Rooms => mRooms.Values;

        #endregion

        #region Getters

        public IChatClient this[int id]
        {
            get
            {
                return GetClient(id);   
            }
        }

        public IChatClient GetClient(int id)
        {
            if (!IsExist(id))
                throw new ArgumentException(nameof(id));

            if (mClients.ContainsKey(id))
                return mClients[id];

            return mRooms[id];
        }

        #endregion

        #region Add/Remove

        public void Add(ChatRoomClient client)
        {
            if (IsExist(client.Id))
                throw new ArgumentException(nameof(client.Id));

            mRooms.Add(client.Id, client);
        }

        public void Add(ChatClient client)
        {
            if (IsExist(client.Id))
                throw new ArgumentException(nameof(client.Id));

            mClients.Add(client.Id, client);
        }

        public void Remove(int id)
        {
            if (mClients.ContainsKey(id))
                mClients.Remove(id);
            else 
                mRooms.Remove(id);
        }

        #endregion

        #region Check methods

        public bool IsExist(int id)
        {
            return mClients.ContainsKey(id) || mRooms.ContainsKey(id);
        }

        #endregion
    }
}
