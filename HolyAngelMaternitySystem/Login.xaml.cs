using MahApps.Metro.Controls;
using NLog;
using System;
using System.Windows;
using System.Windows.Input;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {

        private static Logger Log = LogManager.GetCurrentClassLogger();
        string user;
        public static int userLevel;

        public Login()
        {
            InitializeComponent();
        }

        private void LblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new ForgotPassword().ShowDialog();
            ShowDialog();
        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.FontStyle = FontStyles.Normal;
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                txtPassword.FontStyle = FontStyles.Italic;
            }
            else
            {
                txtUsername.FontStyle = FontStyles.Normal;
            }
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.FontStyle = FontStyles.Normal;
        }

        private void TxtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.FontStyle = FontStyles.Italic;
            }
            else
            {
                txtUsername.FontStyle = FontStyles.Normal;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
            ShowDialog();
        }
    }
}
