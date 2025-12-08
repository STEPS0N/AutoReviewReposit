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
            context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={AuthData.Login};password={AuthData.Password};");
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
                    if (MessageBox.Show($"Удалить {selectedCar.Model_Car}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var car = context.Car.Find(selectedCar.Id_Car);

                        if (car != null)
                        {
                            context.Car.Remove(car);
                            context.SaveChanges();
                            MessageBox.Show("Авто удалено!");
                            LoadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите авто для удаления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        }
    }
}
