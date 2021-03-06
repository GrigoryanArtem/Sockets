﻿// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model.Data
{
    public class ChatUser
    {
        #region Properties

        public int Id { get; private set; }
        public string Name { get; set; }

        #endregion

        public ChatUser(int id, string name = null)
        {
            Name = name ?? String.Empty;
            Id = id;
        }
        
        public override string ToString()
        {
            return $"{Name}#{Id}";
        }

        public override bool Equals(object obj)
        {
            var user = obj as ChatUser;
            return user != null &&
                   Id == user.Id &&
                   Name == user.Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public static ChatUser Parse(string user)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrWhiteSpace(user))
                return null;

            RegexOptions options = RegexOptions.Multiline;

            var match = Regex.Match(user, Constants.UserRegex, options);

            int id = Convert.ToInt32(match.Groups["id"].Value);
            string name = match.Groups["name"].Value;

            return new ChatUser(id, name);
        }
    }
}
