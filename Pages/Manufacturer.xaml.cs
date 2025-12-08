using AutoReview;
using AutoReview.Classes;
using AutoReview.Elements;
using AutoReview.EntityFramework;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoReview.Pages
{
    /// <summary>
    /// Логика взаимодействия для Manufacturer.xaml
    /// </summary>
    public partial class Manufacturer : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;

        public Manufacturer(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={AuthData.Login};password={AuthData.Password};");
            if (AuthData.Rights == false)
            {
                id.Visibility = Visibility.Collapsed;
            }
            LoadData();
        }

        private void LoadData()
        {
            manufacturersList.ItemsSource = context.Manufacturer.ToList();
        }

        private void AddManufacture(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                var window = new Window
                {
                    Title = "Добавить производителя",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new ManufacturerEditControl
                {
                    ManufacturerTitle = "",
                    ManufacturerCountry = "",
                    ManufacturerId = null
                };

                var owners = context.Owners.ToList();
                editControl.LoadOwners(owners);

                editControl.OnSave += (control) =>
                {
                    var manufacturer = new Classes.Manufacturer
                    {
                        Title_Brand = control.ManufacturerTitle,
                        Country_Brand = control.ManufacturerCountry,
                        Owner_Email = control.OwnerEmail
                    };

                    context.Manufacturer.Add(manufacturer);
                    context.SaveChanges();

                    MessageBox.Show($"Производитель {manufacturer.Title_Brand} успешно добавлен!");
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

        private void EditManufacture(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (manufacturersList.SelectedItem is Classes.Manufacturer selected)
                {
                    var window = new Window
                    {
                        Title = "Редактировать производителя",
                        Width = 400,
                        Height = 300,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize
                    };

                    var editControl = new ManufacturerEditControl
                    {
                        ManufacturerTitle = selected.Title_Brand,
                        ManufacturerCountry = selected.Country_Brand,
                        ManufacturerId = selected.Id_Manufacturer,
                        OwnerEmail = selected.Owner_Email
                    };

                    var owners = context.Owners.ToList();
                    editControl.LoadOwners(owners);

                    editControl.OnSave += (control) =>
                    {
                        var manufacturer = context.Manufacturer.Find(control.ManufacturerId);

                        if (manufacturer != null)
                        {
                            manufacturer.Title_Brand = control.ManufacturerTitle;
                            manufacturer.Country_Brand = control.ManufacturerCountry;
                            manufacturer.Owner_Email = control.OwnerEmail;

                            context.SaveChanges();
                            MessageBox.Show("Производитель обновлен!");
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
                    MessageBox.Show("Выберите производителя для редактирования!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете обновлять данные!");
            }
        }

        private void DeleteManufacture(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (manufacturersList.SelectedItem is Classes.Manufacturer selected)
                {
                    if (MessageBox.Show($"Удалить производителя {selected.Title_Brand}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var manufacturer = context.Manufacturer.Find(selected.Id_Manufacturer);

                        if (manufacturer != null)
                        {
                            context.Manufacturer.Remove(manufacturer);
                            context.SaveChanges();
                            MessageBox.Show("Производитель удален!");
                            LoadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите производителя для удаления!");
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




//public partial class Manufacturer : Page
//{
//    public MainWindow mainWindow;

//    public Manufacturer(MainWindow _mainWindow)
//    {
//        InitializeComponent();
//        mainWindow = _mainWindow;
//        LoadData();
//    }

//    // Загрузить данные в таблицу
//    private void LoadData()
//    {
//        using (var context = new AppDbContext($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
//        {
//            usersList.ItemsSource = context.Manufacturer.ToList();
//        }
//    }

//    // КНОПКА ДОБАВИТЬ
//    private void AddUser(object sender, RoutedEventArgs e)
//    {
//        // Создаем окно
//        var window = new Window
//        {
//            Title = "Добавить производителя",
//            Width = 400,
//            Height = 300,
//            WindowStartupLocation = WindowStartupLocation.CenterScreen,
//            ResizeMode = ResizeMode.NoResize
//        };

//        // Создаем UserControl
//        var editControl = new ManufacturerEditControl
//        {
//            ManufacturerTitle = "",
//            ManufacturerCountry = "",
//            ManufacturerId = null
//        };

//        // Сохранить
//        editControl.OnSave += (control) =>
//        {
//            using (var context = new AppDbContext($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
//            {
//                var manufacturer = new Classes.Manufacturer
//                {
//                    Title_Brand = control.ManufacturerTitle,
//                    Country_Brand = control.ManufacturerCountry
//                };

//                context.Manufacturer.Add(manufacturer);
//                context.SaveChanges();

//                MessageBox.Show($"Производитель {manufacturer.Title_Brand} успешно добавлен!");

//                window.Close();
//                LoadData();
//            }
//        };

//        // Отмена
//        editControl.OnCancel += () =>
//        {
//            window.Close();
//        };

//        window.Content = editControl;
//        window.ShowDialog();
//    }

//    // КНОПКА РЕДАКТИРОВАТЬ
//    private void EditUser(object sender, RoutedEventArgs e)
//    {
//        if (usersList.SelectedItem is Classes.Manufacturer selected)
//        {
//            var window = new Window
//            {
//                Title = "Редактировать производителя",
//                Width = 400,
//                Height = 300,
//                WindowStartupLocation = WindowStartupLocation.CenterScreen,
//                ResizeMode = ResizeMode.NoResize
//            };

//            var editControl = new ManufacturerEditControl
//            {
//                ManufacturerTitle = selected.Title_Brand,
//                ManufacturerCountry = selected.Country_Brand,
//                ManufacturerId = selected.Id_Manufacturer
//            };

//            editControl.OnSave += (control) =>
//            {
//                using (var context = new AppDbContext(($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;")))
//                {
//                    var manufacturer = context.Manufacturer.Find(control.ManufacturerId);
//                    if (manufacturer != null)
//                    {
//                        manufacturer.Title_Brand = control.ManufacturerTitle;
//                        manufacturer.Country_Brand = control.ManufacturerCountry;

//                        context.SaveChanges();
//                        MessageBox.Show("Производитель обновлен!");

//                        window.Close();
//                        LoadData();
//                    }
//                }
//            };

//            editControl.OnCancel += () => window.Close();
//            window.Content = editControl;
//            window.ShowDialog();
//        }
//        else
//        {
//            MessageBox.Show("Выберите производителя для редактирования!");
//        }
//    }

//    // КНОПКА УДАЛИТЬ
//    private void DeleteUser(object sender, RoutedEventArgs e)
//    {
//        if (usersList.SelectedItem is Classes.Manufacturer selected)
//        {
//            if (MessageBox.Show($"Удалить производителя {selected.Title_Brand}?",
//                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
//            {
//                using (var context = new AppDbContext(($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;")))
//                {
//                    var manufacturer = context.Manufacturer.Find(selected.Id_Manufacturer);
//                    if (manufacturer != null)
//                    {
//                        context.Manufacturer.Remove(manufacturer);
//                        context.SaveChanges();
//                        MessageBox.Show("Производитель удален!");
//                        LoadData();
//                    }
//                }
//            }
//        }
//        else
//        {
//            MessageBox.Show("Выберите производителя для удаления!");
//        }
//    }

//    // КНОПКА ПОИСК
//    private void Search_Click(object sender, RoutedEventArgs e)
//    {
//        var searchText = search_tb.Text;

//        using (var context = new AppDbContext(($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;")))
//        {
//            if (string.IsNullOrWhiteSpace(searchText))
//            {
//                usersList.ItemsSource = context.Manufacturer.ToList();
//            }
//            else
//            {
//                usersList.ItemsSource = context.Manufacturer
//                    .Where(m => m.Title_Brand.Contains(searchText) ||
//                                m.Country_Brand.Contains(searchText))
//                    .ToList();
//            }
//        }
//    }

//    // КНОПКА В МЕНЮ
//    private void BackMenu(object sender, RoutedEventArgs e)
//    {
//        NavigationService.GoBack();
//    }
//}
