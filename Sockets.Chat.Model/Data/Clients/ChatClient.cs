// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Sockets.Chat.Model.Data.Messages;
using System.Net.Sockets;

namespace Sockets.Chat.Model.Data.Clients
{
    public class ChatClient : DefaultChatClient
    {
        public TcpClient Client { get; set; }

        public ChatClient(int id, string name = null) 
            : base(id, name) { }

        public override void SendMessage(ChatMessage message)
        {
            byte[] buffer = message.ToByteArray();

            lock (Locker)
            {
                NetworkStream stream = Client.GetStream();

                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static ChatClient Create(int id, TcpClient tcpClient, string name = null)
        {
            ChatClient result = new ChatClient(id, name);
            result.Client = tcpClient;

            return result;
        }
    }
}
