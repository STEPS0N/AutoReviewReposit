using AutoReview.Classes;
using AutoReview.EntityFramework;
using AutoReview.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoReview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(pages.authorisation);
        }

        public enum pages
        {
            menu,
            authorisation,
            main,
            manufacturer,
            engine,
            equipment,
            users,
            feedback
        };

        public void OpenPage(pages _pages)
        {
            if (_pages == pages.authorisation)
                frame.Navigate(new Pages.Authorisation(this));
            if (_pages == pages.menu)
                frame.Navigate(new Pages.Menu(this));
            if (_pages == pages.main)
                frame.Navigate(new Pages.Main(this));
            if (_pages == pages.manufacturer)
                frame.Navigate(new Pages.Manufacturer(this));
            if (_pages == pages.engine)
                frame.Navigate(new Pages.Engine(this));
            if (_pages == pages.equipment)
                frame.Navigate(new Pages.Equipment(this));
            if (_pages == pages.users)
                frame.Navigate(new Pages.Users(this));
            if (_pages == pages.feedback)
                frame.Navigate(new Pages.Feedback(this));
        }
    }
}