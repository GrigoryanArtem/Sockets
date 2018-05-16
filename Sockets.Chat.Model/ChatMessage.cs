using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Sockets.Chat.Model
{
    public class ChatMessage
    {
        public MessageCode Code { get; private set; }
        public DateTime Date { get; private set; }
        public string Message { get; private set; }
        public string Sender { get; private set; }
        public string Recipient { get; private set; }

        public ChatMessage(MessageCode code, string sender, DateTime date, string message)
        {
            Code = code;
            Date = date;
            Sender = sender;
            Message = message;
        }

        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(ToString());
        }

        public override string ToString()
        {
            return $"{{{(int)Code}, \"{Sender}\", {Date}, \"{Message}\"}}";
        }

        public static ChatMessage Create(MessageCode code, string sender, DateTime date, string message)
        {
            return new ChatMessage(code, sender, date, message);
        }

        public static ChatMessage Parse(string input)
        {
            RegexOptions options = RegexOptions.Multiline;

            var match = Regex.Match(input, Constants.MessageRegex, options);

            MessageCode code = (MessageCode)Convert.ToInt32(match.Groups["code"].Value);
            DateTime date = DateTime.Parse(match.Groups["date"].Value);
            string sender = match.Groups["sender"].Value;
            string message = match.Groups["message"].Value;

            return Create(code, sender, date, message);
        }
    }
}
