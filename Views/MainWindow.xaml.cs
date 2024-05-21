using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using static Calendar.LoginWindow;
using static Calendar.MainWindow;

namespace Calendar
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public class User
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class Kalendarz
        {
            public int IDkalendarza { get; set; }
            public int UserID { get; set; }
        }

        public class Events
        {
            public int CalendarID { get; set; }
            public int UserID { get; set; } 
            public int EventID { get; set; } 
            public string Title { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        

        private void Window_Funcionality(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EventNameTextBox.Text = "Wpisz nazwę...";
        }

        private void MyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EventNameTextBox.Text == "Nazwij wydarzenie")
            {
                EventNameTextBox.Text = string.Empty;
            }
        }

        private void MyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EventNameTextBox.Text))
            {
                EventNameTextBox.Text = "Nazwij wydarzenie";
            }
        }

        private void FriendTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FriendTextBox.Text == "Podaj nazwę użytkownika")
            {
                FriendTextBox.Text = string.Empty;
            }
        }

        private void FriendTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FriendTextBox.Text))
            {
                FriendTextBox.Text = "Podaj nazwę użytkownika";
            }
        }


        private string connectionString = "Server=LAPTOP-979QAFDU;Database=MeetingsCalendar;Integrated Security=True;";


        private int GetUserID(string userName, SqlConnection connection) //pobiera z bazy id użytkownika sprawdzając nazwę
        {
            string getUserIdQuery = "SELECT UserId FROM Users WHERE UserName = @UserName";
            using (SqlCommand getUserIdCommand = new SqlCommand(getUserIdQuery, connection))
            {
                getUserIdCommand.Parameters.AddWithValue("@UserName", userName);
                return (int)getUserIdCommand.ExecuteScalar();
            }
        }

        private int GetCalendarID(int userID, SqlConnection connection) //pobiera z bazy id kalendarza danego użytkownika
        {
            string getCalendarIdQuery = "SELECT CalendarId FROM Calendars WHERE UserId = @UserId"; //NULLL
            using (SqlCommand getCalendarIdCommand = new SqlCommand(getCalendarIdQuery, connection))
            {
                getCalendarIdCommand.Parameters.AddWithValue("@UserID", userID);
                return (int)getCalendarIdCommand.ExecuteScalar();
            }
        }


        private void LogOut_Click(object sender, RoutedEventArgs e) //Wylogowuje użytkownika
        {

            UserSession.nazwaUzytkownika = null;

            try
            {
                LoginWindow LoginWindow = new LoginWindow();
                LoginWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show("Wystąpił błąd: " + ex.Message); }

            this.Close();

        }


        private void AddEvent_Click(object sender, RoutedEventArgs e) //dodaje wydarzenie
        {
            DateTime eventDate = calendar.SelectedDate ?? DateTime.Today;

            string eventName = EventNameTextBox.Text;

            if( string.IsNullOrWhiteSpace(EventNameTextBox.Text) || EventNameTextBox.Text == "Nazwij wydarzenie")
            {
                MessageBox.Show("Niepoprawna nazwa wydarzenia");
            }
            else
            {
                AddEventToDB(eventDate, eventName);
            }
            EventNameTextBox.Text = "";


        }

        private void AddEventToDB(DateTime data, string title) //dodaje wydarzenie do bazy
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int userID = GetUserID(UserSession.nazwaUzytkownika, connection);
                    int calendarID = GetCalendarID(userID, connection);

                    if (calendarID == -1)
                    {
                        MessageBox.Show("Nie znaleziono kalendarza dla użytkownika.");
                        return;
                    }

                    if (EventExists(calendarID, title, data, connection))
                    {
                        MessageBox.Show("Wydarzenie o tej nazwie już istnieje w tym dniu.", "Błąd", MessageBoxButton.OK);
                        return;
                    }

                    string query = "INSERT INTO Events (CalendarId, UserId, Title, StartDate, EndDate) VALUES (@CalendarId, @UserId, @Title, @StartDate, @EndDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CalendarId", calendarID);
                        command.Parameters.AddWithValue("@UserId", userID);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@StartDate", data);
                        command.Parameters.AddWithValue("@EndDate", data);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas dodawania wydarzenia: " + ex.Message);
                }
            }
        }

        private bool EventExists(int calendarID, string title, DateTime date, SqlConnection connection) //sprawdza czy jest chociaż jedno wydarzenie w danym dniu, zwraca prawde jesli jest
        {
            string checkEventQuery = "SELECT COUNT(*) FROM Events WHERE CalendarId = @CalendarId AND Title = @Title AND StartDate = @StartDate";
            using (SqlCommand checkEventCommand = new SqlCommand(checkEventQuery, connection))
            {
                checkEventCommand.Parameters.AddWithValue("@CalendarId", calendarID);
                checkEventCommand.Parameters.AddWithValue("@Title", title);
                checkEventCommand.Parameters.AddWithValue("@StartDate", date);
                int existingEventsCount = (int)checkEventCommand.ExecuteScalar();
                return existingEventsCount > 0;
            }
        }


        private void Kalendarz_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) //pobiera wydarzenia i dodaje do listy
        {
         
            EventsList.Items.Clear();

            foreach (var selectedDate in calendar.SelectedDates)
            {
            
                List<string> wydarzenia = PobierzWydarzenia(selectedDate, UserSession.nazwaUzytkownika);

                foreach (var wydarzenie in wydarzenia)
                {
                    EventsList.Items.Add(wydarzenie);
                }
            }
        }

        private List<string> PobierzWydarzenia(DateTime date, string userName) //pobiera wydarzenia z bazy dla danego dnia
        {
            List<string> wydarzenia = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Title FROM Events INNER JOIN Users ON Events.UserId = Users.UserId WHERE Events.StartDate = @Date AND Users.UserName = @UserName";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@UserName", userName);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        wydarzenia.Add(reader.GetString(0));
                    }
                }
            }
            return wydarzenia;
        }


        private void DropEvent_Click(object sender, RoutedEventArgs e) //usuwa wydarzenie
        {
            DateTime eventDate = calendar.SelectedDate ?? DateTime.Today;

            string eventName = EventNameTextBox.Text;

            DropEventFromDB(eventDate, eventName);

        }

        private void DropEventFromDB(DateTime data, string title) 
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

               
                connection.Open();

                int userID = GetUserID(UserSession.nazwaUzytkownika, connection);
                int calendarID = GetCalendarID(userID, connection);

                string deleteEventQuery = "DELETE FROM Events WHERE CalendarId = @CalendarId AND Title = @Title AND StartDate = @StartDate";
                using (SqlCommand deleteEventCommand = new SqlCommand(deleteEventQuery, connection))
                {
                    deleteEventCommand.Parameters.AddWithValue("@CalendarId", calendarID);
                    deleteEventCommand.Parameters.AddWithValue("@Title", title);
                    deleteEventCommand.Parameters.AddWithValue("@StartDate", data);
                    deleteEventCommand.ExecuteNonQuery();
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas usuwania wydarzenia: " + ex.Message);
                }
            }

        }


        private List<string> addedUsers = new List<string>();

        private void AddFriend_Click(object sender, RoutedEventArgs e) //dodaje użytkowników
        {
            string friendUserName = FriendTextBox.Text;

            if (CheckIfUserExists(friendUserName))
            {

                if (!string.IsNullOrWhiteSpace(friendUserName))
                {
                    addedUsers.Add(friendUserName);
                    ListOfFriends.Items.Add(friendUserName);
                }
                else
                {
                    MessageBox.Show("Nazwa wydarzenia nie może być pusta.", "Błąd", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Podany użytkownik nie istnieje.", "Błąd", MessageBoxButton.OK);
            }

            FriendTextBox.Text = "";
        }

        private bool CheckIfUserExists(string userName) //sprawdza czy użytkownik istnieje
        {

            string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", userName);
                connection.Open();
                int count = (int)command.ExecuteScalar();

                // Jeśli użytkownik istnieje, zwróć true, w przeciwnym razie false
                return count > 0;
            }
        }

        private void DropFriend_Click(object sender, RoutedEventArgs e) //usuwa użytkownika z listy
        {
            string friendUserName = FriendTextBox.Text;

            if (CheckIfUserExists(friendUserName))
            {

                if (!string.IsNullOrWhiteSpace(friendUserName))
                {
                    ListOfFriends.Items.Remove(friendUserName);
                    addedUsers.Remove(friendUserName);
                }
                else
                {
                    MessageBox.Show("Nazwa wydarzenia nie może być pusta.", "Błąd", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Podany użytkownik nie istnieje.", "Błąd", MessageBoxButton.OK);
            }

            FriendTextBox.Text = "";

        }


        private void ShowFreeDays_Click(object sender, RoutedEventArgs e) //Dodaje optymalne daty spotakań do listy
        {
            MeetingsDays.Items.Clear();
            List<DateTime> SharedDaysOff = FreeDays(ListOfFriends);
            foreach (DateTime date in SharedDaysOff)
            {
                MeetingsDays.Items.Add(date.ToString("dd/MM/yyyy"));
            }

        }

        public List<DateTime> FreeDays(ListBox ListOfFriends) //Przechowuje dni wolne dla wszystkich użytkowników
        {
            List<DateTime> SharedDaysOff = new List<DateTime>();
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    List<int> usersID = new List<int>();
                    foreach (var item in ListOfFriends.Items)
                    {
                        int userID = GetUserID(item.ToString(), connection);
                        if (userID > 0)
                        {
                            usersID.Add(userID);
                        }
                    }

                    int loggedInUserID = GetUserID(UserSession.nazwaUzytkownika, connection);
                    usersID.Add(loggedInUserID);

                    foreach (int userID in usersID)
                    {
                        List<DateTime> FreeDaysList = new List<DateTime>(); // Zdefiniowanie nowej listy na potrzeby każdego użytkownika
                        for (DateTime date = startOfMonth; date <= endOfMonth; date = date.AddDays(1))
                        {
                            if (EventsListIsNull(userID, date))
                            {
                                FreeDaysList.Add(date);
                            }
                        }

                        // Jeśli SharedDaysOff jest puste, to przypisz do niego wszystkie dni z FreeDaysList
                        if (SharedDaysOff.Count == 0)
                        {
                            SharedDaysOff.AddRange(FreeDaysList);
                        }
                        else
                        {
                            // Jeśli SharedDaysOff nie jest puste, to zachowaj tylko te dni, które występują zarówno w SharedDaysOff, jak i FreeDaysList
                            SharedDaysOff = SharedDaysOff.Intersect(FreeDaysList).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }

            return SharedDaysOff;
        }

        private bool EventsListIsNull(int userID, DateTime date) //sprawdza czy dzień wolny
        {
            bool isNull = true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string isThereEventQuery = "SELECT COUNT(UserId) FROM Events WHERE UserId = @UserId AND StartDate = @date";

                using (SqlCommand getDayCommand = new SqlCommand(isThereEventQuery, connection))
                {
                    getDayCommand.Parameters.AddWithValue("@UserId", userID);
                    getDayCommand.Parameters.AddWithValue("@date", date);
                    int numberOfEvents = (int)getDayCommand.ExecuteScalar();

                    if (numberOfEvents != 0)
                    {
                        isNull = false;
                    }
                }

            }

            return isNull;
        }

        
       
    }
}


    



