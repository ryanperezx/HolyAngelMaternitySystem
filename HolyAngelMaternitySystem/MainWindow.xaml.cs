using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PatientRecords pr = new PatientRecords();
        DoctorAnalysis da = new DoctorAnalysis();
        AddPatientRecord apr = new AddPatientRecord();
        ViewPatientRecord vpr = new ViewPatientRecord();
        WelcomePage wp = new WelcomePage();
        Accounts ac = new Accounts();
        UltrasoundReport ur = new UltrasoundReport();
        AddDiagnosisTreatment adt = new AddDiagnosisTreatment();
        public MainWindow()
        {
            InitializeComponent();
            Frame.Navigate(wp);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Do you want to log out?";
            string sCaption = "Logout";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (dr)
            {
                case MessageBoxResult.Yes:
                    this.DialogResult = false;
                    Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }



        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Do you want to exit the application?";
            string sCaption = "Exit";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult dr = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (dr)
            {
                case MessageBoxResult.Yes:
                    this.DialogResult = true;
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward)
            {
                e.Cancel = true;
            }
        }

        private void BtnAccounts_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(ac);
        }

        private void BtnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(pr);
        }

        private void BtnDoctorAnalysis_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(da);
        }

        private void BtnViewPatientRecord_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(vpr);
        }

        private void BtnAddPatientRecord_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(apr);
        }

        private void BtnWelcome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(wp);
        }

        //ultrasound Report
        private void BtnEarlyUltrasound_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(ur);
        }

        private void BtnAddDiagnosisTreatment_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(adt);
        }
    }
}
