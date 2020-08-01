using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientList : Window
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        public PatientList()
        {
            InitializeComponent();
            lvPatientInfo.ItemsSource = records;
        }

        private void TxtPatientID_TextChanged(object sender, TextChangedEventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            if (!string.IsNullOrEmpty(txtPatientID.Text))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where patientID LIKE @patientID", conn))
                {
                    cmd.Parameters.AddWithValue("@patientID", '%' + txtPatientID.Text + '%');

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

                                int age = Convert.ToDateTime(birthDate).Year - DateTime.Today.Year;

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

        }

        private void TxtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            if (!string.IsNullOrEmpty(txtLastName.Text) && !string.IsNullOrEmpty(txtFirstName.Text))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where firstName LIKE @firstName and lastName LIKE @lastName", conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", '%' + txtFirstName.Text + '%');
                    cmd.Parameters.AddWithValue("@lastName",  '%' + txtLastName.Text + '%');

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

                                int age = Convert.ToDateTime(birthDate).Year - DateTime.Today.Year;

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
            else if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where firstName LIKE @firstName", conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", '%' + txtFirstName.Text + '%');
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

                                int age = Convert.ToDateTime(birthDate).Year - DateTime.Today.Year;

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

        }

        private void TxtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where firstName LIKE @firstName and lastName LIKE @lastName", conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", '%' + txtFirstName.Text + '%');
                    cmd.Parameters.AddWithValue("@lastName", '%' + txtLastName.Text + '%');
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

                                int age = Convert.ToDateTime(birthDate).Year - DateTime.Today.Year;

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
            else if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where firstName LIKE @lastName", conn))
                {
                    cmd.Parameters.AddWithValue("@lastName", '%' + txtLastName.Text + '%');
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

                                int age = Convert.ToDateTime(birthDate).Year - DateTime.Today.Year;

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

        }
    }
}
