using System;

namespace Sockets.Chat.Client.GUI.Models
{
    [Flags]
    public enum MessageType
    {
        Default = 0,
        Selected = 1,
        Private = 2       
    }
}
