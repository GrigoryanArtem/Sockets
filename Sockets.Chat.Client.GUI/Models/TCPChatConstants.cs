namespace Sockets.Chat.Client.GUI.Models
{
    internal static class TCPChatConstants
    {
        internal static int DefaultMessageSize => 2048;
        internal static string SelectedMessageRegex = @"@(?<username>[^#]+#\d+)";
        internal static string PrivateMessageRegex = @"^\/p\(@(?<username>[^#]+#\d+)\)";
    }
}
