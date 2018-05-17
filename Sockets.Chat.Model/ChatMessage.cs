using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model
{
    public class ChatMessage
    {
        public MessageCode Code { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public ChatUser Sender { get; set; }
        public ChatUser Recipient { get; set; }

        public ChatMessage(MessageCode code, ChatUser sender, ChatUser recipient, DateTime date, string message)
        {
            Code = code;
            Date = date;
            Sender = sender;
            Recipient = recipient;
            Message = message;
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public override string ToString()
        {
            return $"{{{(int)Code}, \"{Sender}\", \"{Recipient}\", {Date}, \"{Message}\"}}";
        }

        public static ChatMessage Create(MessageCode code, ChatUser sender, ChatUser recipient, DateTime date, string message)
        {
            return new ChatMessage(code, sender, recipient, date, message);
        }

        public static ChatMessage Parse(string input)
        {
            RegexOptions options = RegexOptions.Multiline;

            var match = Regex.Match(input, Constants.MessageRegex, options);

            MessageCode code = (MessageCode)Convert.ToInt32(match.Groups["code"].Value);
            DateTime date = DateTime.Parse(match.Groups["date"].Value);
            ChatUser sender = ChatUser.Parse(match.Groups["sender"].Value);
            ChatUser recipient = ChatUser.Parse(match.Groups["recipient"].Value);
            string message = match.Groups["message"].Value;

            return Create(code, sender, recipient, date, message);
        }
    }
}
