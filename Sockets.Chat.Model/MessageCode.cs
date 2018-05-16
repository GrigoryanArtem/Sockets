namespace Sockets.Chat.Model
{
    public enum MessageCode
    {
        Message = 0,
        Rename = 1,
        CheckConnection = 2,
        ServerName = 3,
        NewUser = 4,
        UserLeave = 5,
        LostConnection = 9999
    }
}
