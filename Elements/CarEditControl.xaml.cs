using AutoReview.Classes;
using System;
using System.Collections.Generic;
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

namespace AutoReview.Elements
{
    /// <summary>
    /// Логика взаимодействия для CarEditControl.xaml
    /// </summary>
    public partial class CarEditControl : UserControl
    {
        public event Action<CarEditControl> OnSave;
        public event Action OnCancel;

        public CarEditControl()
        {
            InitializeComponent();
        }

        public void SetData(List<Manufacturer> manufacturers, List<Engine> engines, Car car = null)
        {
            ManufacturerBox.ItemsSource = manufacturers;
            EngineBox.ItemsSource = engines;

            if (car != null)
            {
                ModelBox.Text = car.Model_Car;
                YearBox.Text = car.Year_Release.ToString();
                PriceBox.Text = car.Price_Car.ToString();

                ManufacturerBox.SelectedValue = car.Manufacturer_Id;
                EngineBox.SelectedValue = car.Engine_Id;

                if (car.Manufacturer != null)
                {
                    ManufacturerBox.SelectedValue = car.Manufacturer.Id_Manufacturer;
                }

                if (car.Engine != null)
                {
                    EngineBox.SelectedValue = car.Engine.Id_Engine;
                }

                foreach (ComboBoxItem item in BodyTypeBox.Items)
                {
                    if (item.Content.ToString() == car.Body_Type)
                    {
                        BodyTypeBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public string Model => ModelBox.Text;
        public string Year => YearBox.Text;
        public string BodyType => BodyTypeBox.Text;
        public string Price => PriceBox.Text;
        public int? ManufacturerId => ManufacturerBox.SelectedValue as int?;
        public int? EngineId => EngineBox.SelectedValue as int?;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManufacturerBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите производителя!");
                return;
            }

            if (string.IsNullOrEmpty(Model))
            {
                MessageBox.Show("Введите модель!");
                return;
            }

            if (!int.TryParse(Year, out int year) || year < 1990)
            {
                MessageBox.Show("Введите корректный год!");
                return;
            }

            if (string.IsNullOrEmpty(BodyType))
            {
                MessageBox.Show("Выберите тип кузова!");
                return;
            }

            if (EngineBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите двигатель!");
                return;
            }

            if (!decimal.TryParse(Price, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену!");
                return;
            }

            OnSave?.Invoke(this);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }
    }
}
