using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        ObservableCollection<PatientRecord> pationtList = new ObservableCollection<PatientRecord>();
    
        QueueSystem qs = new QueueSystem();
        public WelcomePage()
        {
            InitializeComponent();
            lvPatientInfo.ItemsSource = records;
            fillList();
            qs.Show();
        }

        private void fillList()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            //query should be based on patientrecord
            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT TOP 10 pi.patientID, pi.firstName, pi.lastName, pi.homeAddress, pi.civStatus, pi.cellphoneNo, pi.birthDate from tblPersonalInfo pi INNER JOIN tblPatientRecord pr on pi.patientID = pr.patientID", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        records.Clear();
                        while (reader.Read())
                        {

                            int patientIDIndex = reader.GetOrdinal("patientID");
                            string patientID = Convert.ToString(reader.GetValue(patientIDIndex));

                            int firstNameIndex = reader.GetOrdinal("firstName");
                            string firstName = Convert.ToString(reader.GetValue(firstNameIndex));

                            int lastNameIndex = reader.GetOrdinal("lastName");
                            string lastName = Convert.ToString(reader.GetValue(lastNameIndex));

                            int addressIndex = reader.GetOrdinal("homeAddress");
                            string address = Convert.ToString(reader.GetValue(addressIndex));

                            int statusIndex = reader.GetOrdinal("civStatus");
                            string status = Convert.ToString(reader.GetValue(statusIndex));

                            int cellNoIndex = reader.GetOrdinal("cellphoneNo");
                            string cellNo = Convert.ToString(reader.GetValue(cellNoIndex));

                            int birthDateIndex = reader.GetOrdinal("birthDate");
                            string birthDate = Convert.ToString(reader.GetValue(birthDateIndex));

                            int age = Math.Abs(Convert.ToDateTime(birthDate).Year - DateTime.Today.Year) -1;

                            records.Add(new PatientRecord
                            {
                                patientID = patientID,
                                fullName = firstName + " " + lastName,
                                address = address,
                                status = status,
                                contactNo = cellNo,
                                birthDate = birthDate,
                                age = age
                            });


                        }
                    }
                }
            }
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            fillList();
            txtFullName.Text = null;
            txtPatientID.Text = null;
        }

        private void LblSearch_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                MessageBox.Show("Please enter patient ID");
                txtPatientID.Focus();
            }
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
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

                            }
                        }
                        else
                        {
                            MessageBox.Show("Patient does not exist");
                        }
                    }
                }
            }
        }

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }
        private void BtnAddQueue_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtPatientID.Text) && string.IsNullOrEmpty(txtPatientID.Text))
            {
                MessageBox.Show("Patient ID or/and patient name is empty");
            }
            else
            {
                var found = qs.patientList.FirstOrDefault(x => x.patientID == txtPatientID.Text && x.fullName == txtFullName.Text);

                if(found == null)
                {
                    if (qs.patientList.Count == 0)
                    {
                        qs.txtCurrent.Text = txtFullName.Text;
                    }

                    qs.patientList.Add(new PatientRecord
                    {
                        patientID = txtPatientID.Text,
                        fullName = txtFullName.Text
                    });

                    if (qs.patientList.Count > 1 && string.IsNullOrEmpty(qs.txtNext.Text))
                    {
                        qs.txtNext.Text = qs.patientList[qs.patientList.Count - 1].fullName;
                    }

                }
                else
                {
                    MessageBox.Show("Patient is already on the Queue");
                }
            }

        }

        private void BtnRemoveLast_Click(object sender, RoutedEventArgs e)
        {
            if (qs.patientList.Count == 0){
                MessageBox.Show("Queue is empty");
                qs.txtCurrent.Text = null;
            }
            else
            {
                qs.patientList.RemoveAt(0);
                int count = qs.patientList.Count;
                if(count > 3)
                {
                    var found = qs.patientList.SkipWhile(item => item.fullName != qs.txtCurrent.Text).Skip(1).FirstOrDefault();
                    qs.txtCurrent.Text = qs.txtNext.Text;
                    qs.txtNext.Text = found.fullName;
                }
                else if(count == 2)
                {
                    var found = qs.patientList.SkipWhile(item => item.fullName != qs.txtNext.Text).Skip(1).FirstOrDefault();
                    qs.txtCurrent.Text = qs.txtNext.Text;
                    qs.txtNext.Text = found.fullName;
                }
                else if(count == 1)
                {
                    qs.txtCurrent.Text = qs.txtNext.Text;
                    qs.txtNext.Text = null;
                }
                else
                {
                    qs.txtCurrent.Text = null;
                    qs.txtNext.Text = null;
                }
            }
        }
    }
}
