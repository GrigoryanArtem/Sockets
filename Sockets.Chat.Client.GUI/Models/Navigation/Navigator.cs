// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sockets.Chat.Client.GUI.Models.Navigation
{
    public class Navigator
    {
        #region Singleton

        private static volatile Navigator _instance;
        private static object _sync = new Object();

        private Navigator() { }

        private static Navigator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                            _instance = new Navigator();
                    }
                }

                return _instance;
            }
        }

        #endregion

        NavigationService mNavigationService;

        #region Public methods

        public static NavigationService Service
        {
            get
            {
                return Instance.mNavigationService;
            }
            set
            {
                if (Instance.mNavigationService != null)
                    Instance.mNavigationService.Navigated -= Instance.NavigationServiceNavigated;

                Instance.mNavigationService = value;
                Instance.mNavigationService.Navigated += Instance.NavigationServiceNavigated;

            }
        }

        public static void Navigate(Page page, object context)
        {
            if (Instance.mNavigationService is null || page is null)
                return;

            Instance.mNavigationService.Navigate(page, context);
        }

        public static void Navigate(Page page)
        {
            Navigate(page, null);
        }

        #endregion

        #region Private Methods

        private void NavigationServiceNavigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;

            if (page is null)
                return;

            page.DataContext = e.ExtraData;
        }

        #endregion
    }
}
