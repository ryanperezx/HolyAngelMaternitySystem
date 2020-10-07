using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for UltrasoundReport.xaml
    /// </summary>
    public partial class UltrasoundReport : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        public UltrasoundReport()
        {
            InitializeComponent();
            dgList.ItemsSource = records;
            stack.DataContext = new ExpanderListViewModel();

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
            txtPatientID.IsEnabled = true;
            txtFullName.Text = null;
            txtDate.Text = null;

            txt1stTriDays.Text = null;
            txt1stTriEDC.Text = null;
            txt1stTriWeeks.Text = null;
            txt1stCorpus.Text = null;

            txt2ndDays.Text = null;
            txt2ndPlacenta.Text = null;
            txt2ndPresentation.Text = null;
            txt2ndWeeks.Text = null;
            txtFluidVolume.Text = null;
            txt2ndTriEDC.Text = null;

            txtDiagnosis.Document.Blocks.Clear();
            txt1stOthers.Document.Blocks.Clear();
            txt2ndOthers.Document.Blocks.Clear();
            records.Clear();
        }

        private void fillRecord()
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dateVisit, reportType, ultrasoundReport, others from tblPatientRecord where patientID = @patientID ORDER BY CAST(dateVisit AS datetime) ASC", conn))
            {
                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        records.Clear();
                        while (reader.Read())
                        {
                            int reportTypeIndex = reader.GetOrdinal("reportType");
                            string reportType = Convert.ToString(reader.GetValue(reportTypeIndex));

                            int ultrasoundReportIndex = reader.GetOrdinal("ultrasoundReport");
                            string ultrasoundReport = Convert.ToString(reader.GetValue(ultrasoundReportIndex));

                            int dateIndex = reader.GetOrdinal("dateVisit");
                            string date = Convert.ToString(reader.GetValue(dateIndex));

                            int othersIndex = reader.GetOrdinal("others");
                            string others = Convert.ToString(reader.GetValue(othersIndex));

                            records.Add(new PatientRecord
                            {
                                ultrasoundReport = ultrasoundReport,
                                date = date,
                                reportType = reportType,
                                others = others
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

                                txtPatientID.IsEnabled = false;
                                txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

                                txtFullName.Text = firstName + " " + lastName;

                                count = 1;
                                records.Clear();
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
                    string reportType = "";
                    string ultrasoundReport = "";
                    string others1stTri = txt1stTriEDC.Text + "; " + new TextRange(txt1stOthers.Document.ContentStart, txt1stOthers.Document.ContentEnd).Text;
                    string others2ndTri = txt2ndTriEDC.Text + "; " + new TextRange(txt2ndOthers.Document.ContentStart, txt2ndOthers.Document.ContentEnd).Text;

                    //check what tab has text
                    if (cmb1stTrimester.IsChecked == true)
                    {
                        reportType = "1st Trimester";
                        ultrasoundReport = "Pregnancy uterine " + txt1stTriWeeks.Text + " weeks " + txt1stTriDays.Text + " days by CRL" + System.Environment.NewLine + "live singleton" + System.Environment.NewLine + "Normal both ovaries with corpus leteum on " + txt1stCorpus.Text; 
                    }
                    else if (cmb2nd3rdTrimester.IsChecked == true)
                    {
                        reportType = "2nd & 3rd Trimester";
                        ultrasoundReport = "Pregnancy uterine " + txt2ndWeeks.Text + " weeks " + txt2ndDays.Text + " days by fetal biometry" + System.Environment.NewLine + "Live, singleton, in " + txt2ndPresentation.Text + " presentation " + System.Environment.NewLine + txtFluidVolume.Text + " amniotic fluid volume" + System.Environment.NewLine + "Placenta " + txt2ndPlacenta.Text;

                    }
                    else if (cmbGynecology.IsChecked == true)
                    {
                        reportType = "Gynecology";
                        ultrasoundReport = new TextRange(txtDiagnosis.Document.ContentStart, txtDiagnosis.Document.ContentEnd).Text;

                    }
                    else
                    {
                        MessageBox.Show("Please select one of the checkbox before proceeding");
                        return;
                    }
                    string sMessageBoxText = "Confirming Updating Patient Record";
                    string sCaption = "Save Patient Record?";
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (dr)
                    {
                        case MessageBoxResult.Yes:
                            using (SqlCommand cmd = new SqlCommand("UPDATE tblPatientRecord set ultrasoundReport = @ultrasoundReport, reportType = @reportType, others = @others where patientID = @patientID and dateVisit = @date", conn))
                            {
                                cmd.Parameters.AddWithValue("@patientID", txtPatientID.Text);
                                cmd.Parameters.AddWithValue("@date", txtDate.Text);
                                cmd.Parameters.AddWithValue("@reportType", reportType);
                                cmd.Parameters.AddWithValue("@ultrasoundReport", ultrasoundReport);
                                if (cmb1stTrimester.IsChecked == true)
                                {
                                    cmd.Parameters.AddWithValue("@others", others1stTri);
                                }
                                else if (cmb2nd3rdTrimester.IsChecked == true)
                                {
                                    cmd.Parameters.AddWithValue("@others", others2ndTri);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@others", "");
                                }
                                try
                                {
                                    count = cmd.ExecuteNonQuery();
                                    if(count > 0)
                                    {
                                        MessageBox.Show("Record has been updated!");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Patient doesn't have any record on the input date!");
                                    }
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
