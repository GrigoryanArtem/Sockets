using Sockets.Chat.Client.GUI.Models.Navigation;
using Sockets.Chat.Client.GUI.Models.Resources;
using System.Windows;
using System.Windows.Input;

namespace Sockets.Chat.Client.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Navigator.Service = ctrlMainFrame.NavigationService;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ResourcesService.Instance.Unload();
        }
    }
}
