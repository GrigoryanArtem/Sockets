// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Sockets.Chat.Client.GUI.Models
{
    public class RoomsManager : BindableBase
    {
        #region Members

        private Room mCurrentRoom;

        #endregion  
        public int NumberOfUnreadMessages => Rooms.Select(r => r.NumberOfUnreadMessages).Sum();

        public ObservableCollection<Room> Rooms { get; } =
            new ObservableCollection<Room>();

        public Room CurrentRoom
        {
            get => mCurrentRoom;
            set
            {
                mCurrentRoom = value;
                mCurrentRoom.NumberOfUnreadMessages = 0;

                RaisePropertyChanged(nameof(CurrentRoom));
                RaisePropertyChanged(nameof(NumberOfUnreadMessages));
            }
        }

        public void AddMessage(ProxyChatMessage message, Room room)
        {
            if (room is null)
                return;

            IncrementUnreadMessages(room);

            Application.Current.Dispatcher.Invoke(() => {
                room.Messages.Add(message);
            });
        }

        #region Private methods

        private void IncrementUnreadMessages(Room room)
        {
            if (room != CurrentRoom)
            {
                room.NumberOfUnreadMessages++;
                RaisePropertyChanged(nameof(NumberOfUnreadMessages));
            }
        }

        #endregion
    }
}
