using MahApps.Metro.Controls;
using NLog;
using System;
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
            if(IsServerConnected() != true)
            {
                MessageBox.Show("There is no connection with the database, please check your network and see if the device is connected.");
            }

        }

        private static bool IsServerConnected()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        private void LblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new ForgotPassword(txtUsername.Text).ShowDialog();
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
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("One or more fields are empty!");
                return;
            }
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                Nullable<int> loginAttempts;
                using (SqlCommand cmd = new SqlCommand("SELECT tries FROM tblAccounts WHERE username = @username", conn))
                {
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    loginAttempts = Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (loginAttempts < 5)
                {
                    string un = txtUsername.Text;
                    string pw = txtPassword.Password;

                    using (SqlCommand cmd = new SqlCommand("SELECT * from tblAccounts where username = @username AND password = @password", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@password", pw);
                        var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            reader.Close();
                            reader.Dispose();
                            using (SqlCommand cmd2 = new SqlCommand("UPDATE tblAccounts SET tries = 0", conn))
                            {
                                cmd2.ExecuteNonQuery();
                                MessageBox.Show("Login Successful");
                                Log = LogManager.GetLogger("userLogin");
                                Log.Info(" Account Name: " + txtUsername.Text + " has logged in.");
                            }

                        }
                        else
                        {
                            reader.Close();
                            reader.Dispose();
                            using (SqlCommand cmd2 = new SqlCommand("SELECT username from tblAccounts where username = @username", conn))
                            {
                                cmd2.Parameters.AddWithValue("@username", un);
                                SqlDataReader dr = cmd2.ExecuteReader();
                                int ordinal = 0;
                                string value = "";

                                if (dr.Read())
                                {
                                    ordinal = dr.GetOrdinal("username");
                                    value = dr.GetString(ordinal);
                                    if (value.Equals(un))
                                    {
                                        using (SqlCommand cmd3 = new SqlCommand("UPDATE tblAccounts SET tries = tries + 1 WHERE username = @username", conn))
                                        {
                                            cmd3.Parameters.AddWithValue("@username", un);
                                            dr.Close();
                                            dr.Dispose();
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                }

                            }
                            MessageBox.Show("Username or Password is invalid");
                            return;
                        }
                    }
                    Hide();
                    new MainWindow().ShowDialog();
                    ShowDialog();
                    txtPassword.Password = null;
                    txtUsername.Text = null;
                }
                else
                {
                    user = txtUsername.Text;
                    string sMessageBoxText = "Due to multiple login attempts, your account has been locked. \nPlease unlock it to continue.";
                    string sCaption = "Account Recovery";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            Hide();
                            new ForgotPassword(txtUsername.Text).ShowDialog();
                            ShowDialog();
                            break;

                        case MessageBoxResult.No: break;
                    }
                }
            }


        }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
