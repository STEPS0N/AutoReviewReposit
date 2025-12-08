using AutoReview.Classes;
using AutoReview.Pages;
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

        public List<Owner> Owners { get; set; }

        public ManufacturerEditControl()
        {
            InitializeComponent();
        }

        public void LoadOwners(List<Owner> owners)
        {
            Owners = owners;
            OwnerComboBox.ItemsSource = owners;
            OwnerComboBox.DisplayMemberPath = "Fio";
            OwnerComboBox.SelectedValuePath = "Owner_Email";
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

        public string OwnerEmail
        {
            get => OwnerComboBox.SelectedValue?.ToString() ?? "";
            set => OwnerComboBox.SelectedValue = value;
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

            if (OwnerEmail == null)
            {
                MessageBox.Show("Выберите владельца!");
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
