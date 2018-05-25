// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

namespace Sockets.Chat.Model
{
    public enum MessageCode
    {
        Message = 0,
        PublicMessage = 1,
        RepeatMessage = 2,

        Registration = 101,
        Rename = 102,
        ServerUsers = 103,
        ServerName = 104,

        NewUser = 201,
        UserLeave = 202,

        LostConnection = 9999
    }
}
