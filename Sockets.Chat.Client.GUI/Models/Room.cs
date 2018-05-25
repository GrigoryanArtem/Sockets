// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Prism.Mvvm;
using Sockets.Chat.Model.Data;
using System.Collections.ObjectModel;

namespace Sockets.Chat.Client.GUI.Models
{
    public class Room : BindableBase
    {
        #region Members

        private string mName;
        private int mNumberOfUnreadMessages;

        #endregion

        public Room(string name, ChatUser recipient = null)
        {
            Name = name;
            Recipient = recipient;
        }

        public ObservableCollection<ChatUser> Users { get; } 
            = new ObservableCollection<ChatUser>();
        public ObservableCollection<ProxyChatMessage> Messages { get; }
            = new ObservableCollection<ProxyChatMessage>();
        public ChatUser Recipient { get; set; }

        public int NumberOfUnreadMessages
        {
            get => mNumberOfUnreadMessages;
            set
            {
                mNumberOfUnreadMessages = value;
                RaisePropertyChanged(nameof(NumberOfUnreadMessages));
            }
        }

        public string Name
        {
            get => mName;
            set
            {
                mName = value;
                RaisePropertyChanged(nameof(Name));
            }
        }


        public static Room CreateRoomByUser(ChatUser mainUser, params ChatUser[] users)
        {
            var result = new Room(mainUser.Name, mainUser);
            result.Users.AddRange(users);

            return result;
        }

        public static Room CreateSelfRoom(ChatUser user)
        {
            var result = new Room("Saved messages", user);
            result.Users.Add(user);

            return result;
        }
    }
}
