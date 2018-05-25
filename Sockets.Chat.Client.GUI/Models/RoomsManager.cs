using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Sockets.Chat.Client.GUI.Models
{
    public class RoomsManager : BindableBase
    {
        #region Members

        private Room mCurrentRoom;

        #endregion  

        public Room CurrentRoom
        {
            get => mCurrentRoom;
            set
            {
                mCurrentRoom = value;
                RaisePropertyChanged(nameof(CurrentRoom));
            }
        }

        public ObservableCollection<Room> Rooms { get; } =
            new ObservableCollection<Room>();
    }
}
