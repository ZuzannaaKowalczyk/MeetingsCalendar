using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calendar.ViewModels
{
    public class RegistryViewModel
    {
        public void Register(string Username, string Password, string PasswordCheck, string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordCheck))
            {
                if (Password == PasswordCheck)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Sprawdzenie, czy użytkownik o podanej nazwie już istnieje
                            string checkUserQuery = "SELECT COUNT(1) FROM Users WHERE UserName = @Username";
                            SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                            checkUserCommand.Parameters.AddWithValue("@Username", Username);

                            int exists = (int)checkUserCommand.ExecuteScalar();
                            if (exists > 0)
                            {
                                MessageBox.Show("Ta nazwa użytkownika jest już zajęta");
                                return;
                            }

                            // Dodanie nowego użytkownika do bazy danych
                            string insertUserQuery = "INSERT INTO Users (UserName, Password) VALUES (@Username, @Password)";
                            SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection);
                            insertUserCommand.Parameters.AddWithValue("@Username", Username);
                            insertUserCommand.Parameters.AddWithValue("@Password", hashedPassword);
                            insertUserCommand.ExecuteNonQuery();

                            int userID;
                            string getUserIdQuery = "SELECT UserId FROM Users WHERE UserName = @Username";
                            using (SqlCommand getUserIdCommand = new SqlCommand(getUserIdQuery, connection))
                            {
                                getUserIdCommand.Parameters.AddWithValue("@Username", Username);
                                userID = (int)getUserIdCommand.ExecuteScalar();
                            }

                            string insertCalendarQuery = "INSERT INTO Calendars (UserId) VALUES (@UserId)";
                            SqlCommand insertCalendarCommand = new SqlCommand(insertCalendarQuery, connection);
                            insertCalendarCommand.Parameters.AddWithValue("@UserId", userID);
                            insertCalendarCommand.ExecuteNonQuery();

                            MessageBox.Show("Użytkownik zarejestrowany pomyślnie!");

                            OpenLoginWindow();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Wystąpił błąd: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Podane hasła nie są zgodne");
                }
            }
            else
            {
                MessageBox.Show("Nie wypełniłeś wszystkich pól");
            }
        }


        public void OpenLoginWindow()
        {
            try
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                CloseRegistrationWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }

          
        }

        private void CloseRegistrationWindow()
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
