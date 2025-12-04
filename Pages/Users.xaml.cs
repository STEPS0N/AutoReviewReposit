using AutoReview.Classes;
using AutoReview.Elements;
using AutoReview.EntityFramework;
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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;

        public Users(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={AuthData.Login};password={AuthData.Password};");
            LoadData();
        }

        private void LoadData()
        {
            usersList.ItemsSource = context.Users.ToList();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Добавить пользователя",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var editControl = new UserEditControl
            {
                UserLogin = "",
                UserPassword = "",
                UserEmail = "",
                UserId = null
            };

            editControl.OnSave += (control) =>
            {
                var user = new Classes.User
                {
                    Login = control.UserLogin,
                    Password = control.UserPassword,
                    Email_User = control.UserEmail
                };

                context.Users.Add(user);
                context.SaveChanges();

                MessageBox.Show($"Пользователь {user.Login} успешно добавлен!");

                window.Close();
                LoadData();
            };

            editControl.OnCancel += () => window.Close();

            window.Content = editControl;
            window.ShowDialog();
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            if (usersList.SelectedItem is Classes.User selected)
            {
                var window = new Window
                {
                    Title = "Редактировать пользователя",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new UserEditControl
                {
                    UserLogin = selected.Login,
                    UserPassword = "",
                    UserEmail = selected.Email_User,
                    UserId = selected.Id_user
                };

                editControl.OnSave += (control) =>
                {
                    var user = context.Users.Find(control.UserId);

                    if (user != null)
                    {
                        user.Login = control.UserLogin;
                        user.Email_User = control.UserEmail;

                        if (!string.IsNullOrWhiteSpace(control.UserPassword))
                        {
                            user.Password = control.UserPassword;
                        }

                        context.SaveChanges();
                        MessageBox.Show("Пользователь обновлен!");

                        window.Close();
                        LoadData();
                    }
                };

                editControl.OnCancel += () => window.Close();
                window.Content = editControl;
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования!");
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (usersList.SelectedItem is Classes.User selected)
            {
                var result = MessageBox.Show($"Удалить пользователя {selected.Login}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var user = context.Users.Find(selected.Id_user);

                    if (user != null)
                    {
                        context.Users.Remove(user);
                        context.SaveChanges();

                        MessageBox.Show("Пользователь удален!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
