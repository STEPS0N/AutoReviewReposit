using AutoReview.Classes;
using AutoReview.Elements;
using AutoReview.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для Equipment.xaml
    /// </summary>
    public partial class Equipment : Page
    {
        public MainWindow mainWindow;
        private AppDbContext context;

        public Equipment(MainWindow _mainWindow)
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
            try
            {
                var equipmentData = context.Equipment
                    .Include(e => e.Car)
                    .ThenInclude(c => c.Manufacturer)
                    .ToList();

                equipmentList.ItemsSource = equipmentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddEquipment(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                var window = new Window
                {
                    Title = "Добавить комплектацию",
                    Width = 400,
                    Height = 350,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize
                };

                var editControl = new EquipmentEditControl();
                editControl.EquipmentId = null;

                var cars = context.Car.ToList();
                editControl.SetData(cars);

                editControl.OnSave += (control) =>
                {
                    var equipment = new Classes.Equipment
                    {
                        Title_Equipment = control.Title,
                        Equipment_Level = control.Level,
                        Car_Id = control.CarId ?? 0
                    };

                    context.Equipment.Add(equipment);
                    context.SaveChanges();

                    MessageBox.Show("Комплектация добавлена!");
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

        private void EditEquipment(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (equipmentList.SelectedItem is Classes.Equipment selectedEquipment)
                {
                    var window = new Window
                    {
                        Title = "Редактировать комплектацию",
                        Width = 400,
                        Height = 350,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize
                    };

                    var editControl = new EquipmentEditControl();
                    editControl.EquipmentId = selectedEquipment.Id_Equipment;

                    var cars = context.Car.ToList();
                    editControl.SetData(cars, selectedEquipment);

                    editControl.OnSave += (control) =>
                    {
                        var equipment = context.Equipment.Find(selectedEquipment.Id_Equipment);

                        if (equipment != null)
                        {
                            equipment.Title_Equipment = control.Title;
                            equipment.Equipment_Level = control.Level;
                            equipment.Car_Id = control.CarId ?? 0;

                            context.SaveChanges();
                            MessageBox.Show("Комплектация обновлена!");
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
                    MessageBox.Show("Выберите комплектацию!");
                }
            }
            else
            {
                MessageBox.Show("Вы не можете обновлять данные!");
            }
        }

        private void DeleteEquipment(object sender, RoutedEventArgs e)
        {
            if (AuthData.Rights)
            {
                if (equipmentList.SelectedItem is Classes.Equipment selected)
                {
                    string message = $"Удалить комплектацию:\n{selected.Title_Equipment} ({selected.Equipment_Level})?";

                    if (MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var equipment = context.Equipment.Find(selected.Id_Equipment);
                        if (equipment != null)
                        {
                            context.Equipment.Remove(equipment);
                            context.SaveChanges();
                            MessageBox.Show("Комплектация удалена!");
                            LoadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите комплектацию для удаления!");
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
