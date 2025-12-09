using AutoReview;
using AutoReview.Classes;
using AutoReview.Elements;
using AutoReview.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoReview.Pages
{
    /// <summary>
    /// Логика взаимодействия для Engine.xaml
    /// </summary>
    public partial class Engine : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;

        public Engine(MainWindow _mainWindow)
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
            engineList.ItemsSource = context.Engine.ToList();
        }

        private void AddEngine(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                var window = new Window
                {
                    Title = "Добавить двигатель",
                    Height = 300,
                    Width = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new EngineEditControl
                {
                    EngineType = "",
                    EngineCapacity = "",
                    EnginePower = "",
                    EngineId = null
                };

                editControl.OnSave += (control) =>
                {
                    var engine = new Classes.Engine
                    {
                        Type_Engine = control.EngineType,
                        Capacity_Engine = decimal.TryParse(control.EngineCapacity, out decimal capacity) ? capacity : 0,
                        Power_Engine = int.TryParse(control.EnginePower, out int power) ? power : 0
                    };

                    context.Engine.Add(engine);
                    context.SaveChanges();

                    MessageBox.Show($"Двигатель {engine.Type_Engine} успешно добавлен!");

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

        private void EditEngine(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                

                if (engineList.SelectedItem is Classes.Engine selected)
                {
                    var window = new Window
                    {
                        Title = "Редактировать двигатель",
                        Width = 400,
                        Height = 350,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize
                    };

                    var editControl = new EngineEditControl
                    {
                        EngineType = selected.Type_Engine,
                        EngineCapacity = selected.Capacity_Engine.ToString(),
                        EnginePower = selected.Power_Engine.ToString(),
                        EngineId = selected.Id_Engine
                    };

                    editControl.OnSave += (control) =>
                    {

                        var engine = context.Engine.Find(control.EngineId);

                        if (engine != null)
                        {
                            engine.Type_Engine = control.EngineType;
                            engine.Capacity_Engine = decimal.TryParse(control.EngineCapacity, out decimal capacity) ? capacity : 0;
                            engine.Power_Engine = int.TryParse(control.EnginePower, out int power) ? power : 0;

                            context.SaveChanges();
                            MessageBox.Show("Двигатель обновлен!");

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
                    MessageBox.Show("Выберите двигатель для редактирования!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете редактировать данные!");
            }
        }

        private void DeleteEngine(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (engineList.SelectedItem is Classes.Engine selected)
                {
                    var carsWithThisEngine = context.Car.Where(c => c.Engine_Id == selected.Id_Engine).ToList();

                    if (carsWithThisEngine.Count > 0)
                    {
                        string carList = "";
                        foreach (var car in carsWithThisEngine)
                        {
                            carList += $"- {car.Manufacturer?.Title_Brand} {car.Model_Car}\n";
                        }

                        var result = MessageBox.Show(
                            $"ВНИМАНИЕ! Этот двигатель используется в {carsWithThisEngine.Count} автомобилях:\n\n" +
                            carList +
                            "\nПри удалении двигателя эти автомобили тоже удалятся.\n" +
                            "Продолжить?",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            context.Car.RemoveRange(carsWithThisEngine);
                            context.SaveChanges();

                            var engineToDelete = context.Engine.Find(selected.Id_Engine);
                            context.Engine.Remove(engineToDelete);
                            context.SaveChanges();

                            MessageBox.Show($"Удалено: двигатель и {carsWithThisEngine.Count} автомобилей");
                        }
                    }
                    else
                    {
                        if (MessageBox.Show($"Удалить двигатель '{selected.Type_Engine}'?",
                            "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            var engine = context.Engine.Find(selected.Id_Engine);
                            context.Engine.Remove(engine);
                            context.SaveChanges();
                            MessageBox.Show("Двигатель удален!");
                        }
                    }

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Выберите двигатель для удаления!");
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
