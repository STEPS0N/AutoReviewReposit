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
    public partial class Owners : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;

        public Owners(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={AuthData.Login};password={AuthData.Password};");
            LoadData();
        }

        private void LoadData()
        {
            ownersList.ItemsSource = context.Owners.ToList();
        }

        private void AddOwner(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                var window = new Window
                {
                    Title = "Добавить владельца",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new OwnerEditControl
                {
                    OwnerFio = "",
                    OwnerEmail = "",
                    OwnerPhone = "",
                    OwnerId = null
                };

                editControl.OnSave += (control) =>
                {
                    var owner = new Owner
                    {
                        Fio = control.OwnerFio,
                        Email_user = control.OwnerEmail,
                        Phone_number = control.OwnerPhone
                    };

                    context.Owners.Add(owner);
                    context.SaveChanges();

                    MessageBox.Show($"Владелец {owner.Fio} успешно добавлен!");
                    window.Close();
                    LoadData();
                };

                editControl.OnCancel += () => window.Close();
                window.Content = editControl;
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не можете добавлять данные!");
            }
        }

        private void EditOwner(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (ownersList.SelectedItem is Owner selected)
                {
                    var window = new Window
                    {
                        Title = "Редактировать владельца",
                        Width = 400,
                        Height = 250,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize
                    };

                    var editControl = new OwnerEditControl
                    {
                        OwnerFio = selected.Fio,
                        OwnerEmail = selected.Email_user,
                        OwnerPhone = selected.Phone_number,
                        OwnerId = selected.Id_owner
                    };

                    editControl.OnSave += (control) =>
                    {
                        var owner = context.Owners.Find(control.OwnerId);

                        if (owner != null)
                        {
                            owner.Fio = control.OwnerFio;
                            owner.Email_user = control.OwnerEmail;
                            owner.Phone_number = control.OwnerPhone;

                            context.SaveChanges();
                            MessageBox.Show("Владелец обновлен!");
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
                    MessageBox.Show("Выберите владельца для редактирования!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете редактировать данные!");
            }
        }

        private void DeleteOwner(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (ownersList.SelectedItem is Owner selected)
                {
                    if (MessageBox.Show($"Удалить владельца {selected.Fio}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var owner = context.Owners.Find(selected.Id_owner);

                        if (owner != null)
                        {
                            context.Owners.Remove(owner);
                            context.SaveChanges();
                            MessageBox.Show("Владелец удален!");
                            LoadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите владельца для удаления!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете удалять данные!");
            }
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
