// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Sockets.Chat.Model.Data;
using Sockets.Chat.Model.Data.Messages;
using System.Linq;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ProxyChatMessage : ChatMessage
    {
        public ProxyChatMessage(ChatMessage message, bool isCurrentUserMessage)
            : base(message.Code, message.Sender, message.Recipient, message.Date, message.Message)
        {
            IsCurrentUserMessage = isCurrentUserMessage;
        }

        public bool IsCurrentUserMessage { get; private set;}
        public MessageType MessageType { get; private set; }

        internal static ProxyChatMessage CreateMessageByUser(ChatMessage message, ChatUser mUser)
        {
            ProxyChatMessage result = new ProxyChatMessage(message, mUser.Equals(message.Sender))
            {
                MessageType = MessageType.Default
            };

            bool? isMentionedUser = message.Message.MentionedUsers?.Contains(mUser);
            if (isMentionedUser != null && (bool)isMentionedUser)
                result.MessageType = MessageType.Mention;

            return result;
        }
    }
}
