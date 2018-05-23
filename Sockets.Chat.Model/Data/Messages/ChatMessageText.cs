using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model.Data.Messages
{
    public class ChatMessageText
    {
        #region Properties

        public string Message { get; set; }
        public ChatUser[] MentionedUsers { get; set; }
        public ChatUser Recipient { get; set; }

        #endregion

        protected ChatMessageText() { }

        public static ChatMessageText Create(string message)
        {
            ChatMessageText result = new ChatMessageText();
            RegexOptions options = RegexOptions.Multiline;

            var recipientMatch = Regex.Match(message, Constants.RecipientRegex, options);

            if (recipientMatch.Success)
                result.Recipient = ChatUser.Parse(recipientMatch.Groups["username"].Value);

            message = Regex.Replace(message, Constants.RecipientRegex, String.Empty);

            List<ChatUser> mentionedUsersUsers = new List<ChatUser>();
            foreach (Match match in Regex.Matches(message, Constants.MentionedUsersRegex, options))
            {
                var user = ChatUser.Parse(match.Groups["username"].Value);
                
                message = Regex.Replace(message, match.Value, user.Name);
                mentionedUsersUsers.Add(user);
            }

            result.MentionedUsers = mentionedUsersUsers.ToArray();
            result.Message = message;

            return result;
        }

        public static ChatMessageText CreateEmpty()
        {
            return Create(String.Empty);
        }
    }
}
