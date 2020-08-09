using MahApps.Metro.Controls;
using NLog;
using System;
using System.Data.SqlClient;
using System.Windows;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : MetroWindow
    {
        string user, question, answer;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public ForgotPassword(string username)
        {
            InitializeComponent();
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT securityQuestion, answer FROM tblAccounts WHERE username = @username", conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                using (SqlDataReader reader = cmd.ExecuteReader())
                    if (reader.Read())
                    {
                        int securityQuestionIndex = reader.GetOrdinal("securityQuestion");
                        question = Convert.ToString(reader.GetValue(securityQuestionIndex));

                        int answerIndex = reader.GetOrdinal("answer");
                        answer = Convert.ToString(reader.GetValue(answerIndex));

                        securityQuestion.Content = question;
                    }
            }
        }

        private void btnConfirmAns_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAnswer.Text))
            {
                MessageBox.Show("Answer field is empty!");
                txtAnswer.Focus();
            }
            else
            {
                if (txtAnswer.Text.Equals(answer))
                {
                    gridPassword.IsEnabled = true;
                    gridPassword.Visibility = Visibility.Visible;

                    gridSecurity.IsEnabled = false;
                    gridSecurity.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Security answer is wrong");
                }
            }
        }

        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPass.Password) || string.IsNullOrEmpty(txtConfirmPass.Password))
            {
                MessageBox.Show("One or more fields are empty!");
            }
            else
            {
                if (txtNewPass.Password.Equals(txtConfirmPass.Password))
                {
                    string sMessageBoxText = "Are all fields checked?";
                    string sCaption = "Confirm Change Password";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            SqlConnection conn = DBUtils.GetDBConnection();
                            conn.Open();
                            using (SqlCommand cmd1 = new SqlCommand("UPDATE tblAccounts SET password = @password, tries = @tries WHERE username = @username", conn))
                            {
                                cmd1.Parameters.AddWithValue("@username", user);
                                cmd1.Parameters.AddWithValue("@password", txtNewPass.Password);
                                cmd1.Parameters.AddWithValue("@tries", 0);
                                cmd1.ExecuteNonQuery();
                                MessageBox.Show("Password has been changed.");

                                Log = LogManager.GetLogger("AccountLog");
                                Log.Info("Account: " + user + " changed their password");
                            }
                            this.DialogResult = false;

                            break;


                        case MessageBoxResult.No: break;
                    }

                }
                else
                {
                    MessageBox.Show("New password and confirmation password do not match.");
                }
            }
        }
    }
}
