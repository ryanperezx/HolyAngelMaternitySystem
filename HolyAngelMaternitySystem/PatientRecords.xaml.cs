﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HolyAngelMaternitySystem
{
    /// <summary>
    /// Interaction logic for PatientRecords.xaml
    /// </summary>
    public partial class PatientRecords : Page
    {
        public PatientRecords()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LblGenerate_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateTime year = DateTime.Today;
            txtPatientID.Text = Convert.ToString(year.Year) + "-";
        }


        private void LblPatientSearch_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientList pl = new PatientList();
            pl.Show();
        }

        private void LblSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
