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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using NLog;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : MetroWindow
    {
        string user, question, answer;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void BtnConfirmAns_Click(object sender, RoutedEventArgs e)
        {
            gridPassword.IsEnabled = true;
            gridPassword.Visibility = Visibility.Visible;

            gridSecurity.IsEnabled = false;
            gridSecurity.Visibility = Visibility.Collapsed;
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
