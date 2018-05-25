// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Messages;
using System.Threading;

namespace Sockets.Chat.Model.Clients
{
    public interface ITCPChatClient
    {
        void Connect();
        void Disconnect();
        void SendMessage(ChatMessage message);
    }
}