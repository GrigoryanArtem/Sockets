namespace Sockets.Chat.Model
{
    public enum MessageCode
    {
        Message = 0,
        Registration = 1,
        Rename = 2,
        ServerUsers = 3,
        ServerName = 4,
        NewUser = 5,
        UserLeave = 6,
        LostConnection = 9999
    }
}
