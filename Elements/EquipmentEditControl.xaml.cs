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
    /// Логика взаимодействия для EquipmentEditControl.xaml
    /// </summary>
    public partial class EquipmentEditControl : UserControl
    {
        public event Action<EquipmentEditControl> OnSave;
        public event Action OnCancel;

        public EquipmentEditControl()
        {
            InitializeComponent();
        }
        public void SetData(List<Car> cars, Equipment equipment = null)
        {
            CarComboBox.ItemsSource = cars;

            if (equipment != null)
            {
                TitleBox.Text = equipment.Title_Equipment;
                LevelBox.Text = equipment.Equipment_Level;
                CarComboBox.SelectedValue = equipment.Car_Id;
            }
        }

        public string Title => TitleBox.Text;
        public string Level => LevelBox.Text;
        public int? CarId => CarComboBox.SelectedValue as int?;
        public int? EquipmentId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите автомобиль!");
                return;
            }

            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Введите название комплектации!");
                return;
            }

            if (string.IsNullOrWhiteSpace(Level))
            {
                MessageBox.Show("Введите уровень комплектации!");
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
