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
    /// Логика взаимодействия для Engine.xaml
    /// </summary>
    public partial class Engine : Page
    {
        public MainWindow mainWindow;

        public Engine(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {

        }

        private void AddEngine(object sender, RoutedEventArgs e)
        {

        }

        private void EditEngine(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteEngine(object sender, RoutedEventArgs e)
        {

        }
    }
}
