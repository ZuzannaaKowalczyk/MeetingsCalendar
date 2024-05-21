using Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Calendar.LoginWindow;

namespace Calendar
{
    public partial class RegistrationWindow : Window
    {

        private RegistryViewModel ViewModel;

        public RegistrationWindow()
        {
            InitializeComponent();
            ViewModel = new RegistryViewModel();
            DataContext = ViewModel;
        }

        private void Window_Funcionality(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Minimize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private string connectionString = "Server=LAPTOP-979QAFDU;Database=MeetingsCalendar;Integrated Security=True;";


        private void Registy(object sender, RoutedEventArgs e)
        {
            string nazwaUzytkownika = txtUsername.Text;
            string haslo = txtPassword.Password;
            string haslo_spr = txtPasswordCheck.Password;

            ViewModel.Register(nazwaUzytkownika,haslo,haslo_spr,connectionString);
            
            
        }

        private void ToLogin_Window(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenLoginWindow();
        }

    }
}
