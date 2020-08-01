using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for AgeOfGestation.xaml
    /// </summary>
    public partial class AddPatientRecord : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();

        public AddPatientRecord()
        {
            InitializeComponent();
            dgList.ItemsSource = records;
            txtDate.Text = Convert.ToString(DateTime.Today);
        }

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
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

                                computeAgeOfGestation();
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

        private void computeAgeOfGestation()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT ageOfGestation, dateVisit from tblPatientRecord where patientID = @patientID and dateVisit IN (SELECT max(dateVisit) FROM tblPatientRecord)", conn))
            {
                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int aogIndex = reader.GetOrdinal("ageOfGestation");
                        string aog = Convert.ToString(reader.GetValue(aogIndex));

                        int dateIndex = reader.GetOrdinal("dateVisit");
                        DateTime date = Convert.ToDateTime(reader.GetValue(dateIndex));
                    

                        char[] delimiterChars = { ' ', '/'};
                        string[] temp = aog.Split(delimiterChars);

                        double week = Convert.ToInt16(temp[0]);
                        double day = Convert.ToInt16(temp[1]);

                        double difference = (DateTime.Today - date).TotalDays;

                        while(difference >= 7)
                        {
                            week += 1;
                            difference -= -7;
                        }
                        day += difference;

                        txtAOG.Text = week + " " + day + "/7";


                    }
                }
            }
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            records.Clear();
            txtPatientID.Text = null;
            txtBP.Text = null;
            txtDate.Text = null;
            txtFullName.Text = null;
            txtWeight.Text = null;
            txtAOG.Text = null;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(txtBP.Text) || string.IsNullOrEmpty(txtWeight.Text) || string.IsNullOrEmpty(txtAOG.Text))
            {
                MessageBox.Show("One or more fields are empty!");
            }
            else
            {
                //check if patient is existing
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
                    string sMessageBoxText = "Confirming Adding Patient Record";
                    string sCaption = "Save Patient Record?";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            using (SqlCommand cmd = new SqlCommand("INSERT into tblPatientRecord (patientID, dateVisit, bloodPressure, weight, ageOfGestation) VALUES (@patientID, @date, @bloodPressure, @weight, @aog)", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@date", txtDate.Text);
                                cmd.Parameters.AddWithValue("@bloodPressure", txtBP.Text);
                                cmd.Parameters.AddWithValue("@weight", txtWeight.Text);
                                cmd.Parameters.AddWithValue("@aog", txtAOG.Text);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Record has been added!");
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

        private void fillRecord()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dateVisit, weight, bloodPressure, ageOfGestation from tblPatientRecord where patientID = @patientID", conn))
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

                            int weightIndex = reader.GetOrdinal("weight");
                            double weight = Convert.ToDouble(reader.GetValue(weightIndex));

                            int bloodPressureIndex = reader.GetOrdinal("bloodPressure");
                            string bloodPressure = Convert.ToString(reader.GetValue(bloodPressureIndex));

                            int dateIndex = reader.GetOrdinal("dateVisit");
                            DateTime date = Convert.ToDateTime(reader.GetValue(dateIndex));

                            records.Add(new PatientRecord
                            {
                                aog = aog,
                                weight = weight,
                                bloodPressure = bloodPressure,
                                date = date.ToString("MM/dd/yyyy")
                            });

                        }
                    }
                }
            }
        }
    }
}
