using AutoReview.Classes;
using AutoReview.Elements;
using AutoReview.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;
        public List<Car> allCars;

        public Main(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            context = new AppDbContext($"Server=WIN-R32OTPM964O\\SQLEXPRESS;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            //context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={AuthData.Login};password={AuthData.Password};");
            if (AuthData.Rights == false)
            {
                id.Visibility = Visibility.Collapsed;
            }
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var cars = context.Car
                    .Include(c => c.Manufacturer)
                    .Include(c => c.Engine)
                    .ToList();

                carsList.ItemsSource = cars;
                allCars = cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddCar(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                var window = new Window
                {
                    Title = "Добавить авто",
                    Width = 400,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new CarEditControl();

                var manufacturers = context.Manufacturer.ToList();
                var engines = context.Engine.ToList();

                editControl.SetData(manufacturers, engines);

                editControl.OnSave += (control) =>
                {
                    var car = new Car
                    {
                        Model_Car = control.Model,
                        Year_Release = int.Parse(control.Year),
                        Body_Type = control.BodyType,
                        Price_Car = decimal.Parse(control.Price),
                        Manufacturer_Id = control.ManufacturerId ?? 0,
                        Engine_Id = control.EngineId ?? 0
                    };

                    context.Car.Add(car);
                    context.SaveChanges();

                    MessageBox.Show("Авто добавлено!");
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

        private void EditCar(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (carsList.SelectedItem is Car selectedCar)
                {
                    var window = new Window
                    {
                        Title = "Редактировать авто",
                        Width = 400,
                        Height = 500,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    var editControl = new CarEditControl();

                    var manufacturers = context.Manufacturer.ToList();
                    var engines = context.Engine.ToList();

                    editControl.SetData(manufacturers, engines, selectedCar);

                    editControl.OnSave += (control) =>
                    {
                        var car = context.Car.Find(selectedCar.Id_Car);

                        if (car != null)
                        {
                            car.Model_Car = control.Model;
                            car.Year_Release = int.Parse(control.Year);
                            car.Body_Type = control.BodyType;
                            car.Price_Car = decimal.Parse(control.Price);
                            car.Manufacturer_Id = control.ManufacturerId ?? 0;
                            car.Engine_Id = control.EngineId ?? 0;

                            context.SaveChanges();
                            MessageBox.Show("Авто обновлено!");
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
                    MessageBox.Show("Выберите авто!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете обновлять данные!");
            }
        }

        private void DeleteCar(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (carsList.SelectedItem is Car selectedCar)
                {
                    var equipmentList = context.Equipment.Where(e => e.Car_Id == selectedCar.Id_Car).ToList();

                    if (equipmentList.Any())
                    {
                        string equipText = "";
                        foreach (var equip in equipmentList)
                        {
                            equipText += $"- {equip.Title_Equipment} ({equip.Equipment_Level})\n";
                        }

                        string message = $"Удалить автомобиль:\n" +
                                        $"{selectedCar.Manufacturer?.Title_Brand} {selectedCar.Model_Car}\n\n" +
                                        $"Также удалятся комплектации:\n{equipText}\n" +
                                        $"Продолжить?";

                        if (MessageBox.Show(message, "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            context.Equipment.RemoveRange(equipmentList);
                            context.Car.Remove(selectedCar);
                            context.SaveChanges();
                            MessageBox.Show("Удалено!");
                        }
                    }
                    else
                    {
                        string message = $"Удалить автомобиль:\n" +
                                        $"{selectedCar.Manufacturer?.Title_Brand} {selectedCar.Model_Car}?";

                        if (MessageBox.Show(message, "Подтверждение",
                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            context.Car.Remove(selectedCar);
                            context.SaveChanges();
                            MessageBox.Show("Автомобиль удален!");
                        }
                    }

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Выберите авто для удаления!");
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

        private void btn_Search(object sender, RoutedEventArgs e)
        {
            string searchText = tb_search.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                carsList.ItemsSource = allCars;
                return;
            }

            var filteredCars = allCars.Where(car =>

                (car.Manufacturer?.Title_Brand?.ToLower() ?? "").Contains(searchText) ||

                (car.Model_Car?.ToLower() ?? "").Contains(searchText) ||

                (car.Year_Release.ToString()).Contains(searchText) ||

                (car.Body_Type?.ToLower() ?? "").Contains(searchText) ||

                (car.Engine?.Type_Engine?.ToLower() ?? "").Contains(searchText) ||

                car.Price_Car.ToString().Contains(searchText)
            ).ToList();

            carsList.ItemsSource = filteredCars;

            tb_search.Text = "";

            if (!filteredCars.Any())
            {
                MessageBox.Show("Ничего не найдено по вашему запросу.", "Результаты поиска",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
