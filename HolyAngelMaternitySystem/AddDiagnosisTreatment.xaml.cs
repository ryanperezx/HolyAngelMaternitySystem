using NLog;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for AddDiagnosisTreatment.xaml
    /// </summary>
    public partial class AddDiagnosisTreatment : Page
    {
        ObservableCollection<PatientRecord> diagnosisCollection = new ObservableCollection<PatientRecord>();
        ObservableCollection<PatientRecord> treatmentCollection = new ObservableCollection<PatientRecord>();
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public AddDiagnosisTreatment()
        {
            InitializeComponent();
            dgDiagnosis.ItemsSource = diagnosisCollection;
            dgTreatment.ItemsSource = treatmentCollection;
            fillDiagnosis();
            fillTreatment();
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            emptyFields();
        }

        private void emptyFields()
        {
            txtContent.Text = null;
            cmbChoice.SelectedIndex = -1;
            fillDiagnosis();
            fillTreatment();
        }

        private void fillDiagnosis()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT diagnosis from tblDiagnosis", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        diagnosisCollection.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            int diagnosisIndex = reader.GetOrdinal("diagnosis");
                            string diagnosis = Convert.ToString(reader.GetValue(diagnosisIndex));

                            diagnosisCollection.Add(new PatientRecord
                            {
                                diagnosis = diagnosis,
                                i = i
                            });
                            i++;
                        }
                    }
                }
            }
        }

        private void fillTreatment()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT treatment from tblTreatment", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        treatmentCollection.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            int treatmentIndex = reader.GetOrdinal("treatment");
                            string treatment = Convert.ToString(reader.GetValue(treatmentIndex));

                            treatmentCollection.Add(new PatientRecord
                            {
                                treatment = treatment,
                                i = i
                            });
                            i++;
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text))
            {
                MessageBox.Show("Please enter content before saving");
                txtContent.Focus();
            }
            else if (string.IsNullOrEmpty(cmbChoice.Text))
            {
                MessageBox.Show("Please select one of the following choices");
                cmbChoice.Focus();
            }
            else
            {
                string sMessageBoxText = "Confirming addition of Diagnosis or Treatment Record";
                string sCaption = "Add Diagnosis/Treatment?";
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                switch (dr)
                {
                    case MessageBoxResult.Yes:
                        SqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        int count = 0;
                        if (cmbChoice.Text == "Diagnosis")
                        {
                            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblDiagnosis where diagnosis = @diagnosis", conn))
                            {
                                cmd.Parameters.AddWithValue("@diagnosis", txtContent.Text);
                                try
                                {
                                    count = (int)cmd.ExecuteScalar();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }
                            }
                            if (count > 0)
                            {
                                MessageBox.Show("The particular content already exists in the records");
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("INSERT into tblDiagnosis (diagnosis) VALUES (@diagnosis)", conn))
                                {
                                    cmd.Parameters.AddWithValue("@diagnosis", txtContent.Text);
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("Record added successfully");
                                    }
                                    catch (SqlException ex)
                                    {
                                        MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                        Log = LogManager.GetLogger("*");
                                        Log.Error(ex, "Query Error");
                                    }
                                }
                            }

                        }
                        else if (cmbChoice.Text == "Treatment")
                        {

                            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblTreatment where treatment = @treatment", conn))
                            {
                                cmd.Parameters.AddWithValue("@treatment", txtContent.Text);
                                try
                                {
                                    count = (int)cmd.ExecuteScalar();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                    Log = LogManager.GetLogger("*");
                                    Log.Error(ex, "Query Error");
                                }
                            }
                            if (count > 0)
                            {
                                MessageBox.Show("The particular content already exists in the records");
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("INSERT into tblTreatment (treatment) VALUES (@treatment)", conn))
                                {
                                    cmd.Parameters.AddWithValue("@treatment", txtContent.Text);
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("Record added successfully");
                                    }
                                    catch (SqlException ex)
                                    {
                                        MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                                        Log = LogManager.GetLogger("*");
                                        Log.Error(ex, "Query Error");
                                    }
                                }
                            }
                        }

                        emptyFields();
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
