using Newtonsoft.Json;
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
            return JsonConvert.SerializeObject(this);
        }

        public static ChatMessage Create(MessageCode code, ChatUser sender, ChatUser recipient, DateTime date, string message)
        {
            return new ChatMessage(code, sender, recipient, date, message);
        }

        public static ChatMessage Parse(string input)
        {
            return JsonConvert.DeserializeObject<ChatMessage>(input);
        }
    }
}
