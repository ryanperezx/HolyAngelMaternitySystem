using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for EarlyUltrasound.xaml
    /// </summary>
    public partial class EarlyUltrasound : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        public EarlyUltrasound()
        {
            InitializeComponent();
            dgList.ItemsSource = records;
        }

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            emptyField();
        }

        private void emptyField()
        {
            txtPatientID.Text = null;
            txtFullName.Text = null;
            txtUltrasound.Text = null;
            txtDate.Text = null;
            records.Clear();
        }

        private void fillRecord()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dateVisit, ageOfGestation, earlyUltrasound from tblPatientRecord where patientID = @patientID", conn))
            {
                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        records.Clear();
                        while (reader.Read())
                        {
                            int aogIndex = reader.GetOrdinal("ageOfGestation");
                            string aog = Convert.ToString(reader.GetValue(aogIndex));

                            int eutIndex = reader.GetOrdinal("earlyUltrasound");
                            string eut = Convert.ToString(reader.GetValue(eutIndex));

                            int dateIndex = reader.GetOrdinal("dateVisit");
                            string date = Convert.ToString(reader.GetValue(dateIndex));

                            records.Add(new PatientRecord
                            {
                                aog = aog,
                                date = date,
                                eut = eut
                            });

                        }
                    }
                }
            }
        }

        private void LblSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                txtPatientID.Focus();
            }
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                int count = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT firstName, lastName from tblPersonalInfo where patientID = @patientID", conn))
                {
                    cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                int firstNameIndex = reader.GetOrdinal("firstName");
                                string firstName = Convert.ToString(reader.GetValue(firstNameIndex));

                                int lastNameIndex = reader.GetOrdinal("lastName");
                                string lastName = Convert.ToString(reader.GetValue(lastNameIndex));

                                txtFullName.Text = firstName + " " + lastName;

                                count = 1;

                                txtUltrasound.Text = null;
                                txtDate.Text = null;

                            }
                        }
                        else
                        {
                            MessageBox.Show("Patient does not exist");
                        }
                    }
                }
                if (count > 0)
                {
                    fillRecord();
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(txtUltrasound.Text))
            {
                MessageBox.Show("One or more fields are empty!");
            }
            else
            {
                int count = 0;
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblPersonalInfo where patientID = @patientID", conn))
                {
                    cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                    try
                    {
                        count = (int)cmd.ExecuteScalar();

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                    }
                }
                if (count > 0)
                {
                    string sMessageBoxText = "Confirming Updating Patient Record";
                    string sCaption = "Save Patient Record?";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            using (SqlCommand cmd = new SqlCommand("UPDATE tblPatientRecord set earlyUltrasound = @eut where patientID = @patientID and dateVisit = @date", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@date", txtDate.Text);
                                cmd.Parameters.AddWithValue("@eut", txtUltrasound.Text);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Record has been updated!");
                                    fillRecord();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            return;
                        case MessageBoxResult.Cancel:
                            return;
                    }
                }
                else
                {
                    MessageBox.Show("Patient does not exist.");
                }
            }
        }
    }
}
