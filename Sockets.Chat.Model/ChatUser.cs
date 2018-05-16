using System;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model
{
    public class ChatUser
    {
        #region Members

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion


        public ChatUser(int id, string name = null)
        {
            Name = name ?? String.Empty;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}#{Name}";
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
