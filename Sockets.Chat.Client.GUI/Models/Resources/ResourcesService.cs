using System;

namespace Sockets.Chat.Client.GUI.Models.Resources
{
    public class ResourcesService
    {
        #region Singleton

        private static volatile ResourcesService _instance;
        private static object _sync = new Object();

        private ResourcesService() { }

        public static ResourcesService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                            _instance = new ResourcesService();
                    }
                }

                return _instance;
            }
        }

        #endregion

        public event EventHandler Unloading;

        public void Unload()
        {
            Unloading?.Invoke(null, new EventArgs());
        }
    }
}
