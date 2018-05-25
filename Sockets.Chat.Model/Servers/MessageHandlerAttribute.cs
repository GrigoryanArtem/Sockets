// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;

namespace Sockets.Chat.Model.Servers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MessageHandlerAttribute : Attribute
    {
        public MessageCode MessageCode { get; private set; }

        public MessageHandlerAttribute() { }

        public MessageHandlerAttribute(MessageCode messageCode)
        {
            MessageCode = messageCode;
        }
    }
}