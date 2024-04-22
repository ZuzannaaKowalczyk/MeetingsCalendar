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
using System.Data.Entity;
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
            public List<Events> List { get; set; } // Lista wydarzeń przypisanych do użytkownika
        }

        public class Kalendarz
        {
            public int IDkalendarza { get; set; }
            public int UserID { get; set; }  
        }

        public class Events
        {
            public int CalendarID { get; set; }
            public int UserID { get; set; } // ID użytkownika, do którego należy wydarzenie
            public int EventID { get; set; } // ID wydarzenia
            public string Title { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
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

    }


    


}
