using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calendar.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        
        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        
        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            //weryfikacja danych logowania

            MainViewModel mainViewModel = new MainViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            Application.Current.MainWindow.Close(); 
            mainWindow.Show();
        }
    }
}
