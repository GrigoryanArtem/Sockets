// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using NLog;
using Sockets.Chat.Model.Data.Clients;
using Sockets.Chat.Model.Data.Messages;

namespace Sockets.Chat.Model.Data
{
    public class ChatMail
    {
        #region Members
        
        private ChatClients mClients;
        private ILogger mLogger;

        #endregion

        public ChatMail(ChatClients clients)
        {
            mClients = clients;
            mLogger = LogManager.GetCurrentClassLogger();
        }

        public void SendMessage(ChatMessage message, int clientId)
        {
            mLogger.Trace($"{message}");

            if (mClients.IsExist(clientId))
                mClients[clientId].SendMessage(message);
        }

        public void SendMessage(ChatMessage message)
        {
            mLogger.Trace($"{message}");

            foreach (var user in mClients.RegestredUsers)
                user.SendMessage(message);

        }
    }
}
