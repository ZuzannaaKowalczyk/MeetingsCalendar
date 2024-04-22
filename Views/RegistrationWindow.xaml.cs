// File: RegistrationWindow.xaml.cs
using System.Windows;
using System.Windows.Media;

namespace Calendarz
{
    public partial class RegistrationWindow : Window
    {
        private const string UsernamePlaceholder = "Username";
        private const string PasswordPlaceholder = "Password";

        public RegistrationWindow()
        {
            InitializeComponent();
            ResetFields();
        }

        private void ResetFields()
        {
            txtUsername.Text = UsernamePlaceholder;
            txtPassword.Password = "";
            txtPassword.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == UsernamePlaceholder)
            {
                txtUsername.Text = "";
                txtUsername.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = UsernamePlaceholder;
                txtUsername.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Foreground = new SolidColorBrush(Colors.Black);
            if (txtPassword.Password == PasswordPlaceholder)
            {
                txtPassword.Password = "";
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                txtPassword.Password = PasswordPlaceholder;
                txtPassword.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        
    }
}
