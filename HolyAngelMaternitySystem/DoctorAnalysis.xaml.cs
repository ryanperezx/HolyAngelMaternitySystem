using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for ViewPatientRecords.xaml
    /// </summary>
    public partial class DoctorAnalysis : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        public DoctorAnalysis()
        {
            InitializeComponent();
            dgList.ItemsSource = records;
            dgList.MinRowHeight = 50;
            populateDiagTreatment();

        }

        private void populateDiagTreatment()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT treatment from tblDiagnosis", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        cmbDiagnosis.Items.Clear();
                        while (reader.Read())
                        {
                            int treatmentIndex = reader.GetOrdinal("treatment");
                            string treatment = Convert.ToString(reader.GetValue(treatmentIndex));

                            cmbDiagnosis.Items.Add(treatment);
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
                using (SqlCommand cmd = new SqlCommand("SELECT patientID, firstName, lastName from tblPersonalInfo where patientID = @patientID", conn))
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
                            fillList();
                        }
                        else
                        {
                            MessageBox.Show("Patient does not exist");
                        }
                    }
                }
            }

        }

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            records.Clear();
            txtFullName.Text = null;
            txtPatientID.Text = null;
            cmbDiagnosis.Text = null;
            txtDate.Text = null;
            txtEUT.Text = null;
            cmbDiagnosis.SelectedIndex = -1;
            txtFindings.Document.Blocks.Clear();
            populateDiagTreatment();
        }

        private void fillList()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * from tblPatientRecord where patientID = @patientID", conn))
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

                            int findingsIndex = reader.GetOrdinal("findings");
                            string findings = Convert.ToString(reader.GetValue(findingsIndex));

                            int diagnosisIndex = reader.GetOrdinal("diagnosisTreatment");
                            string diagnosis = Convert.ToString(reader.GetValue(diagnosisIndex));

                            int eutIndex = reader.GetOrdinal("endoscopicUltrasoundTomography");
                            string eut = Convert.ToString(reader.GetValue(eutIndex));

                            records.Add(new PatientRecord
                            {
                                aog = aog,
                                weight = weight,
                                bloodPressure = bloodPressure,
                                date = date.ToString("MM/dd/yyyy"),
                                eut = eut,
                                diagnosis = diagnosis,
                                findings = findings
                            });
                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string Findings = new TextRange(txtFindings.Document.ContentStart, txtFindings.Document.ContentEnd).Text;
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please search for the patient ID first");
            }
            else if (string.IsNullOrEmpty(cmbDiagnosis.Text))
            {
                MessageBox.Show("Please enter/select diagnosis or treatment");
                cmbDiagnosis.Focus();
            }
            else if (string.IsNullOrEmpty(Findings))
            {
                MessageBox.Show("Please enter your findings");
                txtFindings.Focus();
            }
            else
            {
                int count = 0;
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) from tblDiagnosis where treatment = @treatment", conn))
                {
                    cmd.Parameters.AddWithValue("@treatment", cmbDiagnosis.Text);
                    try
                    {
                        count = (int)cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                    }
                }
                if (count == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT into tblDiagnosis (treatment) VALUES (@treatment)", conn))
                    {
                        cmd.Parameters.AddWithValue("@treatment", cmbDiagnosis.Text);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            populateDiagTreatment();

                            var found = records.FirstOrDefault(x => Convert.ToDateTime(txtDate.Text) == Convert.ToDateTime(x.date));
                            if (found != null)
                            {
                                if (!string.IsNullOrEmpty(txtEUT.Text))
                                {
                                    found.eut = txtEUT.Text;
                                }

                                found.diagnosis = cmbDiagnosis.Text;
                                found.findings = Findings;

                            }
                            else
                            {
                                MessageBox.Show("There are no past records in the indicated date.");
                                MessageBox.Show(txtDate.Text);
                                return;
                            }


                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("An error has been encountered! Log has been updated with the error " + ex);
                        }
                    }
                }
                else
                {
                    var found = records.FirstOrDefault(x => Convert.ToDateTime(txtDate.Text) == Convert.ToDateTime(x.date));
                    if (found != null)
                    {
                        if (!string.IsNullOrEmpty(txtEUT.Text))
                        {
                            found.eut = txtEUT.Text;
                        }

                        found.diagnosis = cmbDiagnosis.Text;
                        found.findings = Findings;
                    }
                    else
                    {
                        MessageBox.Show("There are no past records in the indicated date.");
                        MessageBox.Show(txtDate.Text);
                        return;
                    }


                }
                dgList.Items.Refresh();

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please search for the Patient ID");
            }
            else
            {
                string sMessageBoxText = "Confirming update of Patient Record";
                string sCaption = "Update Patient Record";
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                switch (dr)
                {
                    case MessageBoxResult.Yes:
                        SqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        var lastSet = records.Last();
                        if(Convert.ToDateTime(txtDate.Text) != Convert.ToDateTime(lastSet.date))
                        {
                            MessageBox.Show("Dates are not equal!");
                            return;
                        }
                        using (SqlCommand cmd = new SqlCommand("UPDATE tblPatientRecord SET findings = @findings, endoscopicUltrasoundTomography = @eut, diagnosisTreatment = @diagnosis where patientID = @patientID and dateVisit = @date", conn))
                        {
                            cmd.Parameters.AddWithValue("@findings", lastSet.findings);
                            cmd.Parameters.AddWithValue("@eut", lastSet.eut);
                            cmd.Parameters.AddWithValue("@diagnosis", lastSet.diagnosis);
                            cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                            cmd.Parameters.AddWithValue("@date", lastSet.date);
                            try
                            {

                                int count = cmd.ExecuteNonQuery();
                                MessageBox.Show("Record has been added!");
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
        }
    }
}
