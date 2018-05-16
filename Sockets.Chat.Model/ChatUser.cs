using System;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model
{
    public class ChatUser
    {
        #region Members

        public int Id { get; private set; }
        public string Name { get; private set; }

        #endregion


        public ChatUser(int id, string name)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}#{Name}";
        }

        public static ChatUser Parse(string user)
        {
            RegexOptions options = RegexOptions.Multiline;

            var match = Regex.Match(user, Constants.UserRegex, options);

            int id = Convert.ToInt32(match.Groups["id"].Value);
            string name = match.Groups["name"].Value;

            return new ChatUser(id, name);
        }
    }
}
