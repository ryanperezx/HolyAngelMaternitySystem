using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NLog;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for PatientRecords.xaml
    /// </summary>
    public partial class PatientRecords : Page
    {
        int state = 0;
        private static Logger Log = LogManager.GetCurrentClassLogger();
        public PatientRecords()
        {
            InitializeComponent();
            cmbCivStatus.Items.Add("Single");
            cmbCivStatus.Items.Add("Married");
            cmbCivStatus.Items.Add("Divorced");
            cmbCivStatus.Items.Add("Separated");
            cmbCivStatus.Items.Add("Widowed");
            
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            state = 1;
            lblGenerate.Visibility = Visibility.Visible;
            lblSearch.Visibility = Visibility.Collapsed;
            txtPatientID.IsReadOnly = true;
            txtFirstName.IsReadOnly = false;
            txtLastName.IsReadOnly = false;
            txtFirstName.Text = null;
            txtLastName.Text = null;
            txtAddress.Text = null;
            txtBirthDate.Text = null;
            txtCellNo.Text = null;
            txtFirstName.Text = null;
            txtLastName.Text = null;
            cmbCivStatus.SelectedIndex = -1;

            btnAdd.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (state == 0)
            {
                MessageBox.Show("Please click the Add button if you are adding new patient information");
            }
            else if(state == 1)
            {
                if (string.IsNullOrEmpty(txtPatientID.Text))
                {
                    MessageBox.Show("Please click Generate to generate ID");
                    lblGenerate.Focus();
                }
                else if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(cmbCivStatus.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtBirthDate.Text) || string.IsNullOrEmpty(txtCellNo.Text))
                {
                    MessageBox.Show("One or more field are empty");
                }
                else
                {
                    string sMessageBoxText = "Confirming Adding Patient Information";
                    string sCaption = "Save Patient Information";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            SqlConnection conn = DBUtils.GetDBConnection();
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("INSERT into tblPersonalInfo (patientID, firstName, lastName, homeAddress, civStatus, birthdate, cellphoneNo) VALUES (@patientID, @firstName, @lastName, @address, @civStatus, @birthDate, @cellNo)", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                                cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@civStatus", cmbCivStatus.Text);
                                cmd.Parameters.AddWithValue("@birthDate", txtBirthDate.Text);
                                cmd.Parameters.AddWithValue("@cellNo", txtCellNo.Text);

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Patient Information has been added!");
                                    Log = LogManager.GetLogger("patientInfo");
                                    Log.Info("Patient:  " + txtPatientID.Text + " has been registered!");
                                    emptyFields();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }

                            }
                            break;
                        case MessageBoxResult.No:
                            return;
                        case MessageBoxResult.Cancel:
                            return;
                    }

                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                MessageBox.Show("Please search for Patient ID before updating");
                txtPatientID.Focus();
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(cmbCivStatus.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtBirthDate.Text) || string.IsNullOrEmpty(txtCellNo.Text))
            {
                MessageBox.Show("One or more field are empty");
            }
            else
            {
                if(state == 2)
                {
                    string sMessageBoxText = "Confirming update of Patient Information";
                    string sCaption = "Update Patient Information";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            SqlConnection conn = DBUtils.GetDBConnection();
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("UPDATE tblPersonalInfo SET homeAddress = @address, birthDate = @birthDate, civStatus = @civStatus, cellphoneNo = @cellNo where patientID = @patientID", conn))
                            {
                                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@civStatus", cmbCivStatus.Text);
                                cmd.Parameters.AddWithValue("@birthDate", txtBirthDate.Text);
                                cmd.Parameters.AddWithValue("@cellNo", txtCellNo.Text);
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Patient Information has been updated!");
                                    Log = LogManager.GetLogger("patientInfo");
                                    Log.Info("Patient:  " + txtPatientID.Text + " has been updated!");
                                    emptyFields();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
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
                    MessageBox.Show("Please search the patient ID first before clicking on edit button");
                }

            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                MessageBox.Show("Please enter Patient ID before deleting");
                txtPatientID.Focus();
            }
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                int count = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblPersonalInfo where patientID = @patientID", conn))
                {
                    cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                    try
                    {
                        count = (int)cmd.ExecuteScalar();

                    }
                    catch(SqlException ex)
                    {
                        MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                        Log = LogManager.GetLogger("*");
                        Log.Error(ex, "Query Error");
                    }
                }
                if(count == 0)
                {
                    MessageBox.Show("Patient does not exist.");
                }
                else
                {
                    string sMessageBoxText = "Confirming deletion of Patient Information";
                    string sCaption = "Delete Patient Information";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            using (SqlCommand cmd = new SqlCommand("DELETE from tblPersonalInfo where patientID = @patientID", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }
                            }
                            using (SqlCommand cmd = new SqlCommand("DELETE from tblPatientRecord where patientID = @patientID", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }
                            }

                            MessageBox.Show("Patient Information has been deleted!");

                            break;
                        case MessageBoxResult.No:
                            return;
                        case MessageBoxResult.Cancel:
                            return;
                    }

                }
            }
        }

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }

        private void LblGenerate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateTime year = DateTime.Today;

            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT TOP(1) patientID from tblPersonalInfo ORDER BY id DESC", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    int countIndex = reader.GetOrdinal("patientID");
                    string count = Convert.ToString(reader.GetValue(countIndex));

                    char[] delimiterChars = { '-'};
                    string[] temp = count.Split(delimiterChars);

                    double finalCount = Convert.ToInt16(temp[1]);
                    txtPatientID.Text = Convert.ToString(year.Year) + "-" + (finalCount + 1);
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
                using (SqlCommand cmd = new SqlCommand("SELECT firstName, lastName, homeAddress, civStatus, cellphoneNo, birthDate from tblPersonalInfo where patientID = @patientID", conn))
                {
                    cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                int firstNameIndex = reader.GetOrdinal("firstName");
                                txtFirstName.Text = Convert.ToString(reader.GetValue(firstNameIndex));

                                int lastNameIndex = reader.GetOrdinal("lastName");
                                txtLastName.Text = Convert.ToString(reader.GetValue(lastNameIndex));

                                int addressIndex = reader.GetOrdinal("homeAddress");
                                txtAddress.Text = Convert.ToString(reader.GetValue(addressIndex));

                                int statusIndex = reader.GetOrdinal("civStatus");
                                cmbCivStatus.Text = Convert.ToString(reader.GetValue(statusIndex));

                                int cellNoIndex = reader.GetOrdinal("cellphoneNo");
                                txtCellNo.Text = Convert.ToString(reader.GetValue(cellNoIndex));

                                int birthDateIndex = reader.GetOrdinal("birthDate");
                                txtBirthDate.Text = Convert.ToString(reader.GetValue(birthDateIndex));

                                txtPatientID.IsEnabled = false;

                                txtFirstName.IsReadOnly = true;
                                txtLastName.IsReadOnly = true;

                                btnEdit.IsEnabled = true;
                                btnDelete.IsEnabled = true;
                                btnAdd.IsEnabled = false;

                                state = 2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Patient information does not exist");
                        }
                    }

                }
            }
        }
        private void emptyFields()
        {
            lblGenerate.Visibility = Visibility.Collapsed;
            lblSearch.Visibility = Visibility.Visible;
            txtPatientID.IsEnabled = true;
            txtPatientID.IsReadOnly = false;

            state = 0;

            txtAddress.Text = null;
            txtFirstName.Text = null;
            txtLastName.Text = null;
            txtBirthDate.Text = null;
            cmbCivStatus.SelectedIndex = -1;
            txtPatientID.Text = null;
            txtCellNo.Text = null;

            btnAdd.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            emptyFields();
        }
    }
}
