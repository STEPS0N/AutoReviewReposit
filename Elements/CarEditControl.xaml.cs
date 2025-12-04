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

        public List<Manufacturer> Manufacturers { get; set; }
        public List<Engine> Engines { get; set; }

        public Manufacturer SelectedManufacturer { get; set; }
        public Engine SelectedEngine { get; set; }

        public CarEditControl()
        {
            InitializeComponent();
        }

        // Метод для загрузки данных в контрол
        public void LoadData(List<Manufacturer> manufacturers, List<Engine> engines, Car car = null)
        {
            Manufacturers = manufacturers;
            Engines = engines;

            ManufacturerBox.ItemsSource = manufacturers;
            ManufacturerBox.DisplayMemberPath = "Title_Brand";

            EngineBox.ItemsSource = engines;
            EngineBox.DisplayMemberPath = "Type_Engine";

            if (car != null)
            {
                ModelBox.Text = car.Model_Car;
                YearBox.Text = car.Year_Release.ToString();
                PriceBox.Text = car.Price_Car.ToString();

                foreach (Manufacturer manufacture in ManufacturerBox.Items)
                {
                    if (manufacture.Id_Manufacturer == car.Manufacturer_Id)
                    {
                        ManufacturerBox.SelectedItem = manufacture;
                        break;
                    }
                }

                foreach (Engine engine in EngineBox.Items)
                {
                    if (engine.Id_Engine == car.Engine_Id)
                    {
                        EngineBox.SelectedItem = engine;
                        break;
                    }
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

        public Car GetCarData()
        {
            return new Car
            {
                Model_Car = ModelBox.Text,
                Year_Release = int.Parse(YearBox.Text),
                Body_Type = BodyTypeBox.Text,
                Price_Car = decimal.Parse(PriceBox.Text),
                Manufacturer = ManufacturerBox.SelectedItem as Manufacturer,
                Engine = EngineBox.SelectedItem as Engine
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManufacturerBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите производителя!");
                return;
            }

            if (string.IsNullOrEmpty(ModelBox.Text))
            {
                MessageBox.Show("Введите модель!");
                return;
            }

            if (!int.TryParse(YearBox.Text, out int year) || year < 1990)
            {
                MessageBox.Show("Введите корректный год!");
                return;
            }

            if (string.IsNullOrEmpty(BodyTypeBox.Text))
            {
                MessageBox.Show("Выберите тип кузова!");
                return;
            }

            if (EngineBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите двигатель!");
                return;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
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
