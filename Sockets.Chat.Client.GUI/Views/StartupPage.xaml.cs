using Sockets.Chat.Client.GUI.ViewModels;
using System.Windows.Controls;

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
