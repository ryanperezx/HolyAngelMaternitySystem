using MahApps.Metro.Controls;
using NLog;
using System.Data.SqlClient;
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
            new ForgotPassword("admin").ShowDialog();
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
            /*
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblAccounts where id", conn))
            {
                int userCount = (int)cmd.ExecuteScalar();
                if (userCount == 0)
                {
                    MessageBox.Show("Empty!");
                } 
                else
                {
                    Hide();
                    new MainWindow().ShowDialog();
                    ShowDialog();
                }
            }
            */
            Hide();
            new MainWindow().ShowDialog();
            ShowDialog();

        }
    }
}
