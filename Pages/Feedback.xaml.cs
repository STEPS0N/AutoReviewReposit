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

namespace AutoReview.Pages
{
    /// <summary>
    /// Логика взаимодействия для Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        public MainWindow mainWindow;

        public Feedback(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {

        }

        private void AddUser(object sender, RoutedEventArgs e)
        {

        }

        private void EditUser(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {

        }
    }
}
