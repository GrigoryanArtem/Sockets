﻿namespace Sockets.Chat.Model
{
    internal static class Constants
    {
        internal static string DefaultName => "user#";
        internal static string MessageRegex => @"{(?<code>\d{1,4}), ""(?<sender>[^""]*?)"", ""(?<recipient>[^""]*?)"", (?<date>\d{2}\.\d{2}\.\d{4} \d{1,2}:\d{1,2}:\d{1,2}), ""(?<message>[^""]*?)""}";
        internal static string UserRegex => @"^(?<name>.*?)#(?<id>\d+)$";
        internal static int ServerId => 0;
        internal static int FirstUserId => 1;
        internal static int UserIdIncrementValue => 1;
        internal static int ConnectionTimeout => 1;
        internal static int MessageSize => 2048;
    }
}
