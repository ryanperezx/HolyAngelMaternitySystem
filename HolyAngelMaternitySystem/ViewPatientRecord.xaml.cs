﻿using Syncfusion.DocIO.DLS;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for ViewPatientRecord.xaml
    /// </summary>
    public partial class ViewPatientRecord : Page
    {
        ObservableCollection<PatientRecord> records = new ObservableCollection<PatientRecord>();
        public ViewPatientRecord()
        {
            InitializeComponent();
            lvPatientInfo.ItemsSource = records;
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
                using (SqlCommand cmd = new SqlCommand("SELECT * from tblPersonalInfo where patientID = @patientID", conn))
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

                                int addressIndex = reader.GetOrdinal("homeAddress");
                                txtAddress.Text = Convert.ToString(reader.GetValue(addressIndex));

                                int statusIndex = reader.GetOrdinal("civStatus");
                                txtCivStatus.Text = Convert.ToString(reader.GetValue(statusIndex));

                                int cellNoIndex = reader.GetOrdinal("cellphoneNo");
                                txtCellNo.Text = Convert.ToString(reader.GetValue(cellNoIndex));

                                int birthDateIndex = reader.GetOrdinal("birthDate");
                                txtBirthDate.Text = Convert.ToString(reader.GetValue(birthDateIndex));

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

        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }

        private void LblReset_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            records.Clear();
            txtPatientID.Text = null;
            txtFullName.Text = null;
            txtCivStatus.Text = null;
            txtCellNo.Text = null;
            txtBirthDate.Text = null;
            txtAddress.Text = null;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            WordDocument document = new WordDocument();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (document)
            {
                IWSection section = document.AddSection();
                IWTextRange text;

                IWParagraph title = section.AddParagraph();
                IWParagraph doc;
                IWParagraph personalInfo;
                IWParagraph firstPersonalInfo;
                IWParagraph secondPersonalInfo;
                IWParagraph columnNames;

                document.AddSection();
                WTextBody sectionBody = section.Body;
                text = title.AppendText("HOLY ANGELS MATERNITY AND POLYCLINIC");
                title.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                text.CharacterFormat.FontName = "Arial";
                text.CharacterFormat.FontSize = 14;
                text.CharacterFormat.Bold = true;
                title = section.AddParagraph();
                text = title.AppendText("M. Palad Street, Poblacion, Norzagaray, Bulacan");
                title.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                text.CharacterFormat.FontSize = 10;
                text.CharacterFormat.Bold = true;
                title = section.AddParagraph();

                doc = section.AddParagraph();
                text = doc.AppendText("SHEILA MARIE C. GIRON-SANTOS, M.D., FPOGS");
                doc.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                text.CharacterFormat.FontName = "Times New Roman";
                text.CharacterFormat.FontSize = 16;
                doc = section.AddParagraph();
                doc.AppendText("Obstetrician-Gynecologist");
                doc.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                text.CharacterFormat.FontSize = 8;
                doc = section.AddParagraph();
                doc.AppendText("Fellow, Philippine Obstetrical & Gynecological Society");
                doc.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;


                personalInfo = section.AddParagraph();
                text = personalInfo.AppendText("Patient Record");
                text.CharacterFormat.FontName = "Arial";
                text.CharacterFormat.FontSize = 16;
                text.CharacterFormat.Bold = true;
                personalInfo.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                //set line color to green

                firstPersonalInfo = section.AddParagraph();
                text = firstPersonalInfo.AppendText("Patient's Name: " + txtFullName.Text + " Age: " + computeAge(txtBirthDate.Text) + " Status: " + txtCivStatus.Text + " Birthdate: " + txtBirthDate.Text);
                text.CharacterFormat.FontSize = 10;
                text.CharacterFormat.Bold = false;
                secondPersonalInfo = section.AddParagraph();
                text = secondPersonalInfo.AppendText("Home Address: " + txtAddress.Text + " Cellphone No: " + txtCellNo.Text);
                columnNames = section.AddParagraph();
                text = columnNames.AppendText("Date WT BP AOG EUT OB-GYNE HISTORY,P.E. AND LABORATORY FINDINGS DIAGNOSIS/TREATMENT");




                document.Save(path + "\\[" + txtFullName.Text + "]" + "Patient Record.docx");

            }
            document.Close();

        }

        private int computeAge(string dateOfBirth)
        {
            DateTime birthDate = Convert.ToDateTime(dateOfBirth);
            DateTime Now = DateTime.Now;
            int year = new DateTime(DateTime.Now.Subtract(birthDate).Ticks).Year - 1;
            return year;
        }
    }
}
