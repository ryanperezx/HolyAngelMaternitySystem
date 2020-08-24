using System;
using System.Windows;
using System.Windows.Forms;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for QueueSystem.xaml
    /// </summary>
    public partial class QueueSystem : Window
    {
        public QueueSystem()
        {
            InitializeComponent();
        }

        private void ShowOnMonitor(int monitor, Window window)
        {
            var screen = ScreenHandler.GetScreen(monitor);
            var currentScreen = ScreenHandler.GetCurrentScreen(this);
            window.WindowState = WindowState.Normal;
            window.Left = screen.WorkingArea.Left;
            window.Top = screen.WorkingArea.Top;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
            window.Loaded += Window_Loaded;
        }

        /*You can use this event for all the Windows*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var senderWindow = sender as Window;
            ShowOnMonitor(1, senderWindow);
            senderWindow.WindowState = WindowState.Maximized;
        }
    }
}
