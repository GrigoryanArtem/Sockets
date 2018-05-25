// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ChatService
    {
        #region Singleton

        private static volatile ChatService _instance;
        private static object _sync = new Object();

        private ChatService() { }

        private static ChatService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                            _instance = new ChatService();
                    }
                }

                return _instance;
            }
        }

        #endregion

        private ChatModel mModel = new ChatModel();

        public static ChatModel CurrentChatModel
        {
            get
            {
                return Instance.mModel;
            }
        }
    }
}
