namespace Sockets.Chat.Client.GUI.Models
{
    internal static class TCPChatConstants
    {
        internal static int DefaultMessageSize => 4096;
        internal static string SelectedMessageRegex = @"@(?<username>[^#]+#\d+)";
        internal static string PrivateMessageRegex = @"^\/p\(@(?<username>[^#]+#\d+)\)";
    }
}
