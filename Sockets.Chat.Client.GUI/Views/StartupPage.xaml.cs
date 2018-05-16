﻿using Sockets.Chat.Client.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sockets.Chat.Client.GUI.Views
{
    /// <summary>
    /// Логика взаимодействия для StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        public StartupPage()
        {
            InitializeComponent();
            DataContext = new StartupPageViewModel();
        }
    }
}
