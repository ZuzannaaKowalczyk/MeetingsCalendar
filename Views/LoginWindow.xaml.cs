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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calendar
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel ViewModel;

        private string connectionString = "Server=LAPTOP-979QAFDU;Database=MeetingsCalendar;Integrated Security=True;";

        public LoginWindow()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
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

        private void Log_In(object sender, RoutedEventArgs e)
        {
            UserSession.nazwaUzytkownika = txtUsername.Text;
            string nazwaUzytkownika = txtUsername.Text;
            string haslo = txtPassword.Password;
            ViewModel.Login(nazwaUzytkownika,haslo,connectionString);
        }

        public class UserSession
        {
            public static string nazwaUzytkownika { get; set; }
        }


        private void ToRegister_Window(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenRegistrationWindow();
        }

    }
}
