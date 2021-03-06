﻿// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Runtime.Serialization;

namespace Sockets.Chat.Model.Clients
{
    public class ServerUnavailableException : Exception
    {
        public ServerUnavailableException() { }

        public ServerUnavailableException(string message)
            : base(message) { }

        public ServerUnavailableException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ServerUnavailableException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
