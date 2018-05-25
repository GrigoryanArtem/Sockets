// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

namespace Sockets.Chat.Client.GUI.Models.ValidationRules
{
    internal static class ValidationConstants
    {
        public static string IpAddressRegex => @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";

        internal static string  UserNameRegex => @"^(?=.{4,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";
    }
}
