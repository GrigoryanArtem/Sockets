// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Prism.Mvvm;
using Sockets.Chat.Model.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sockets.Chat.Client.GUI.Models
{
    public class ChatRoom : BindableBase
    {
        #region Members

        private string mName;
        private List<ChatUser> Recipients = new List<ChatUser>();
        public string Name
        {
            get => mName;
            set
            {
                mName = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<ProxyChatMessage> Messages { get; private set; }
            = new ObservableCollection<ProxyChatMessage>();

        #endregion

        #region Properties



        #endregion
    }
}
