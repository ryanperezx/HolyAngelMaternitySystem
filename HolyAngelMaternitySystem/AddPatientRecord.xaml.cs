using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using NLog;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for AgeOfGestation.xaml
    /// </summary>
    public partial class AddPatientRecord : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        private static Logger Log = LogManager.GetCurrentClassLogger();

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
                MessageBox.Show("Please enter patient ID");
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

                                txtAOG.Text = null;
                                txtBP.Text = null;
                                txtDate.Text = null;
                                txtWeight.Text = null;
                                txtUltrasound.Text = null;
                                txtLMP.Text = null;

                                records.Clear();

                                //computeAgeOfGestation();
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


                        char[] delimiterChars = { ' ', '/' };
                        string[] temp = aog.Split(delimiterChars);
                        double week = Convert.ToInt16(temp[0]);
                        double day = Convert.ToInt16(temp[1]);

                        double difference = (DateTime.Today - date).TotalDays;

                        while (difference >= 7)
                        {
                            week += 1;
                            difference -= 7;
                        }
                        day += difference;

                        if (day >= 7)
                        {
                            week += 1;
                            day -= 7;
                        }

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
            txtFullName.Text = null;
            txtWeight.Text = null;
            txtAOG.Text = null;
            txtUltrasound.Text = null;
            txtLMP.Text = null;
            txtOBIndex.Document.Blocks.Clear();
            txtDate.Text = DateTime.Today.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtDate.Text))
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
                    string sMessageBoxText = "Confirming Adding Patient Record";
                    string sCaption = "Save Patient Record?";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            using (SqlCommand cmd = new SqlCommand("INSERT into tblPatientRecord (patientID, dateVisit, bloodPressure, weight, ageOfGestation, earlyUltrasound, LMP) VALUES (@patientID, @date, @bloodPressure, @weight, @aog, @eut, @LMP)", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@date", txtDate.Text);
                                cmd.Parameters.AddWithValue("@bloodPressure", txtBP.Text);
                                cmd.Parameters.AddWithValue("@weight", txtWeight.Text);
                                cmd.Parameters.AddWithValue("@aog", txtAOG.Text);
                                cmd.Parameters.AddWithValue("@eut", txtUltrasound.Text);
                                cmd.Parameters.AddWithValue("@LMP", txtLMP.Text);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Record has been added!");
                                    Log = LogManager.GetLogger("patientRecord");
                                    Log.Info("Patient:  " + txtPatientID.Text + " has added an record!");
                                    fillRecord();
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
                    MessageBox.Show("Patient does not exist.");
                }

            }

        }

        private void fillRecord()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dateVisit, weight, bloodPressure, ageOfGestation, earlyUltrasound, LMP from tblPatientRecord where patientID = @patientID", conn))
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

                            int eutIndex = reader.GetOrdinal("earlyUltrasound");
                            string eut = Convert.ToString(reader.GetValue(eutIndex));

                            int lmpIndex = reader.GetOrdinal("LMP");
                            string lmp = Convert.ToString(reader.GetValue(lmpIndex));

                            records.Add(new PatientRecord
                            {
                                aog = aog,
                                eut = eut,
                                weight = weight,
                                bloodPressure = bloodPressure,
                                date = date.ToString("MM/dd/yyyy"),
                                lmp = lmp
                            });

                        }
                    }
                }
            }
        }

        private void BtnOBIndex_Click(object sender, RoutedEventArgs e)
        {
            string obIndex = new TextRange(txtOBIndex.Document.ContentStart, txtOBIndex.Document.ContentEnd).Text;
            if (string.IsNullOrEmpty(txtPatientID.Text) && string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please search for the patient ID first.");
                txtPatientID.Focus();
            }
            else if (string.IsNullOrEmpty(obIndex))
            {
                MessageBox.Show("OB-Index field is empty");
                txtOBIndex.Focus();
            }
            else
            {
                string sMessageBoxText = "Confirming Update/Adding OB Index";
                string sCaption = "Save OB Index?";
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                switch (dr)
                {
                    case MessageBoxResult.Yes:
                        int count = 0;
                        SqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblObIndex where patientID = @patientID", conn))
                        {
                            cmd.Parameters.AddWithValue("@obIndex", obIndex);
                            cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                            count = (int)cmd.ExecuteScalar();
                        }
                        if (count > 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("UPDATE tblObIndex set obIndex = @obIndex where patientID = patientId", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@obIndex", obIndex);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Updated OB-Index Record");
                                }
                                catch (SqlException ex)
                                {

                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }

                            }
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT into tblObIndex (patientID, obIndex) VALUES (@patientID, @obIndex)", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@obIndex", obIndex);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Added OB-Index Record");
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }
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
}
