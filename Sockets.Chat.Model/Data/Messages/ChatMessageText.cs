// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model.Data.Messages
{
    public class ChatMessageText
    {
        #region Properties

        public string Text { get; set; }
        public ChatUser[] MentionedUsers { get; set; }

        #endregion

        protected ChatMessageText() { }

        public static ChatMessageText Create(string message)
        {
            ChatMessageText result = new ChatMessageText();
            RegexOptions options = RegexOptions.Multiline;
           
            List<ChatUser> mentionedUsersUsers = new List<ChatUser>();
            foreach (Match match in Regex.Matches(message, Constants.MentionedUsersRegex, options))
            {
                var user = ChatUser.Parse(match.Groups[Constants.UsernameRegexGroup].Value);
                
                message = Regex.Replace(message, match.Value, user.Name);
                mentionedUsersUsers.Add(user);
            }

            result.MentionedUsers = mentionedUsersUsers.ToArray();
            result.Text = message;

            return result;
        }

        public static ChatMessageText CreateEmpty()
        {
            return Create(String.Empty);
        }
    }
}
