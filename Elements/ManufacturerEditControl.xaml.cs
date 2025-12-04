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
    /// Логика взаимодействия для ManufacturerEditControl.xaml
    /// </summary>
    public partial class ManufacturerEditControl : UserControl
    {
        public event Action<ManufacturerEditControl> OnSave;
        public event Action OnCancel;

        public ManufacturerEditControl()
        {
            InitializeComponent();
        }

        public string ManufacturerTitle
        {
            get => TitleBox.Text;
            set => TitleBox.Text = value;
        }

        public string ManufacturerCountry
        {
            get => CountryBox.Text;
            set => CountryBox.Text = value;
        }

        public int? ManufacturerId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ManufacturerTitle))
            {
                MessageBox.Show("Введите название производителя!");
                return;
            }

            if (string.IsNullOrWhiteSpace(ManufacturerCountry))
            {
                MessageBox.Show("Введите страну производителя!");
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
