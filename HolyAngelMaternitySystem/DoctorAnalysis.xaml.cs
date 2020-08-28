﻿using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NLog;
using System.Text.RegularExpressions;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for ViewPatientRecords.xaml
    /// </summary>
    public partial class DoctorAnalysis : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        private static Logger Log = LogManager.GetCurrentClassLogger();
        public DoctorAnalysis()
        {
            InitializeComponent();
            dgList.ItemsSource = records;
            dgList.MinRowHeight = 50;
            populateDiagnosis();
            populateTreatment();

        }

        private void populateDiagnosis()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT diagnosis from tblDiagnosis", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        cmbDiagnosis.Items.Clear();
                        while (reader.Read())
                        {
                            int diagnosisIndex = reader.GetOrdinal("diagnosis");
                            string diagnosis = Convert.ToString(reader.GetValue(diagnosisIndex));

                            cmbDiagnosis.Items.Add(diagnosis);
                        }
                    }
                }
            }
        }

        private void populateTreatment()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT treatment from tblTreatment", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        cmbTreatment.Items.Clear();
                        while (reader.Read())
                        {
                            int treatmentIndex = reader.GetOrdinal("treatment");
                            string treatment = Convert.ToString(reader.GetValue(treatmentIndex));

                            cmbTreatment.Items.Add(treatment);
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
            cmbTreatment.Text = null;
            txtDate.Text = DateTime.Today.ToString();
            cmbDiagnosis.SelectedIndex = -1;
            cmbTreatment.SelectedIndex = -1;
            txtFindings.Document.Blocks.Clear();
            txtFH.Text = null;
            txtFHT.Text = null;
            populateTreatment();
            populateDiagnosis();
        }

        private void fillList()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * from tblPatientRecord pr LEFT JOIN tblObIndex oi on pr.patientID = oi.patientID where pr.patientID = @patientID ", conn))
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
                            string date = Convert.ToString(reader.GetValue(dateIndex));

                            int findingsIndex = reader.GetOrdinal("findings");
                            string findings = Convert.ToString(reader.GetValue(findingsIndex));

                            int fhIndex = reader.GetOrdinal("fh");
                            string fh = Convert.ToString(reader.GetValue(fhIndex));

                            int fhtIndex = reader.GetOrdinal("fht");
                            string fht = Convert.ToString(reader.GetValue(fhtIndex));

                            int obIndexIndex = reader.GetOrdinal("obIndex");
                            string obIndex = Convert.ToString(reader.GetValue(obIndexIndex));

                            int diagnosisIndex = reader.GetOrdinal("diagnosis");
                            string diagnosis = Convert.ToString(reader.GetValue(diagnosisIndex));

                            int treatmentIndex = reader.GetOrdinal("treatment");
                            string treatment = Convert.ToString(reader.GetValue(treatmentIndex));

                            int eutIndex = reader.GetOrdinal("earlyUltrasound");
                            string eut = Convert.ToString(reader.GetValue(eutIndex));

                            int lmpIndex = reader.GetOrdinal("LMP");
                            string lmp = Convert.ToString(reader.GetValue(lmpIndex));

                            int ultrasoundReportIndex = reader.GetOrdinal("ultrasoundReport");
                            string ultrasoundReport = Convert.ToString(reader.GetValue(ultrasoundReportIndex));

                            int othersIndex = reader.GetOrdinal("others");
                            string others = Convert.ToString(reader.GetValue(othersIndex));

                            int reportTypeIndex = reader.GetOrdinal("reportType");
                            string reportType = Convert.ToString(reader.GetValue(reportTypeIndex));

                            records.Add(new PatientRecord
                            {
                                aog = aog,
                                weight = weight,
                                bloodPressure = bloodPressure,
                                fht = fht,
                                fh = fh,
                                date = date,
                                eut = eut,
                                lmp = lmp,
                                obIndex = obIndex,
                                diagnosis = diagnosis,
                                treatment = treatment,
                                findings = findings,
                                ultrasoundReport = ultrasoundReport,
                                reportType = reportType,
                                others = others
                            });
                        }
                    }
                }
            }
        }


        private void onlyNumbers(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9\.]+");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string Findings = new TextRange(txtFindings.Document.ContentStart, txtFindings.Document.ContentEnd).Text;
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please search for the patient ID first");
            }
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                MessageBox.Show("Please enter date that is to be updated");
            }
            else if(string.IsNullOrEmpty(cmbDiagnosis.Text) && string.IsNullOrEmpty(cmbTreatment.Text) && string.IsNullOrEmpty(Findings))
            {
                MessageBox.Show("One or more fields are empty!");
            }
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                var found = records.FirstOrDefault(x => Convert.ToDateTime(txtDate.Text) == Convert.ToDateTime(x.date));
                if (found != null)
                {
                    found.diagnosis = cmbDiagnosis.Text;
                    found.treatment = cmbTreatment.Text;
                    found.findings = Findings;
                    found.fh = txtFH.Text;
                    found.fht = txtFHT.Text;
                }
                else
                {
                    MessageBox.Show("There are no past record of the indicated date.");
                    return;
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
                        var found = records.FirstOrDefault(x => Convert.ToDateTime(txtDate.Text) == Convert.ToDateTime(x.date));
                        using (SqlCommand cmd = new SqlCommand("UPDATE tblPatientRecord SET findings = @findings, diagnosis = @diagnosis, treatment = @treatment, fh = @fh, fht = @fht where patientID = @patientID and dateVisit = @date", conn))
                        {
                            cmd.Parameters.AddWithValue("@findings", found.findings);
                            cmd.Parameters.AddWithValue("@diagnosis", found.diagnosis);
                            cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                            cmd.Parameters.AddWithValue("@treatment", found.treatment);
                            cmd.Parameters.AddWithValue("@date", found.date);
                            cmd.Parameters.AddWithValue("@fh", found.fh);
                            cmd.Parameters.AddWithValue("@fht", found.fht);
                            try
                            {

                                int count = cmd.ExecuteNonQuery();
                                if (count > 0)
                                {
                                    MessageBox.Show("Record has been updated!");
                                    Log = LogManager.GetLogger("patientRecord");
                                    Log.Info("Patient:  " + txtPatientID.Text + " has been updated with doctor's analysis!");

                                }
                                else
                                {
                                    MessageBox.Show("Patient doesn't have any record on the input date!");
                                }
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
}
