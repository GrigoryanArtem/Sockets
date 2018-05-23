namespace Sockets.Chat.Model
{
    internal static class Constants
    {
        internal static string UserRegex => @"^(?<name>.*?)#(?<id>\d+)$";
        internal static int ServerId => 0;
        internal static int FirstUserId => 1;
        internal static int UserIdIncrementValue => 1;
        internal static int ConnectionTimeout => 1;
        internal static int MessageSize => 4096;
        internal static string MentionedUsersRegex => @"@(?<username>[^#]+#\d+)";
        internal static string RecipientRegex => @"^>(?<username>.+?#\d+)";
    }
}
