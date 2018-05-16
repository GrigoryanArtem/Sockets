using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Sockets.Chat.Model
{
    public class ChatClients
    {
        #region Members

        private Dictionary<int, TcpClient> mClients = new Dictionary<int, TcpClient>();
        private Dictionary<int, string> mClientsNames = new Dictionary<int, string>();
        private HashSet<int> mIsRegisteredUsers = new HashSet<int>();

        #endregion

        #region Properties

        public IEnumerable<TcpClient> Clients => mClients.Values;
        public IEnumerable<TcpClient> RegestredClients => mClients
            .Where(client => IsRegistered(client.Key))
            .Select(client => client.Value);
        public IEnumerable<ChatUser> RegestredUsers => mClientsNames
            .Where(client => IsRegistered(client.Key))
            .Select(client => new ChatUser(client.Key, client.Value));

        #endregion

        #region Getters

        public TcpClient this[int id]
        {
            get
            {
                return GetClient(id);   
            }
        }

        public TcpClient GetClient(int id)
        {
            if (!IsExist(id))
                throw new ArgumentException(nameof(id));

            return mClients[id];
        }

        public IEnumerable<TcpClient> GetClients(params int[] ids)
        {
            return mClients
                .Where(client => ids.Contains(client.Key))
                .Select(client => client.Value);
        }

        public IEnumerable<TcpClient> GetRegisteredClients(params int[] ids)
        {
            var realIds = ids
                .Intersect(mIsRegisteredUsers.ToArray())
                .ToArray();

            return GetClients(realIds);
        }

        public IEnumerable<TcpClient> GetExistingClients(params int[] ids)
        {
            var realIds = ids
                .Intersect(mClients.Keys)
                .ToArray(); ;

            return GetClients(realIds);
        }

        public string GetName(int id)
        {
            if (!IsRegistered(id))
                throw new ArgumentException(nameof(id));

            return mClientsNames[id];
        }

        #endregion

        #region Add/Remove

        public void Add(int id, TcpClient client)
        {
            if (IsExist(id))
                throw new ArgumentException(nameof(id));

            mClients.Add(id, client);
        }

        public void Add(int id, TcpClient client, string name)
        {
            Add(id, client);
            Rename(id, name);
        }

        public void Remove(int id)
        {
            if (IsRegistered(id))
            {
                mClientsNames.Remove(id);
                mIsRegisteredUsers.Remove(id);
            }

            mClients.Remove(id);
        }

        #endregion

        public void Registration(int id, string name)
        {
            Rename(id, name);
        }

        public void Rename(int id, string name)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (IsRegistered(id))
            {
                mClientsNames[id] = name;
            }
            else
            {
                mIsRegisteredUsers.Add(id);
                mClientsNames.Add(id, name);
            }
        }

        #region Check methods

        public bool IsExist(int id)
        {
            return mClients.ContainsKey(id);
        }

        public bool IsRegistered(int id)
        {
            if (!IsExist(id))
                throw new ArgumentException(nameof(id));

            return mIsRegisteredUsers.Contains(id);
        }

        #endregion
    }
}
