using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calendar.ViewModels
{
    public class LoginViewModel
    {

        public void Login(string Username, string Password, string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                string nazwaUzytkownika = Username;
                string haslo = Password;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sql = "SELECT Password FROM Users WHERE UserName = @NazwaUzytkownika";

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@NazwaUzytkownika", nazwaUzytkownika);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string hashedPassword = result.ToString();

                            // Weryfikacja hasła
                            if (BCrypt.Net.BCrypt.Verify(haslo, hashedPassword))
                            {
                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                CloseLoginWindow();
                            }
                            else
                            {
                                MessageBox.Show("Nieprawidłowa nazwa użytkownika lub hasło.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nieprawidłowa nazwa użytkownika lub hasło.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Wprowadź nazwę użytkownika i hasło.");
                
            }
        }


        public void OpenRegistrationWindow()
        {
            try
            {
                RegistrationWindow registrationWindow = new RegistrationWindow();
                registrationWindow.Show();
                CloseLoginWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }

        private void CloseLoginWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
