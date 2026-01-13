using AutoReview.Classes;
using AutoReview.EntityFramework;
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

        //Метод инициализации выпадающего списка владельцев
        public void LoadOwners(List<Owner> owners)
        {
            Owners = owners;
            OwnerComboBox.ItemsSource = owners;
            OwnerComboBox.DisplayMemberPath = "Fio";
            OwnerComboBox.SelectedValuePath = "Owner_Email";
        }

        // Основные свойства
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

        //Метод для сохранения изменений и валидации данных
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

            using (var context = new AppDbContext($"Server=WIN-R32OTPM964O\\SQLEXPRESS;Database=AutoReview;User Id={AuthData.Login};" +
                $"Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
            {
                bool alreadyExists = context.Manufacturer.Any(man => man.Title_Brand == ManufacturerTitle || man.Owner_Email == OwnerEmail);

                if (ManufacturerId.HasValue)
                {
                    alreadyExists = context.Manufacturer.Any(man => man.Title_Brand == ManufacturerTitle 
                    || man.Owner_Email == OwnerEmail || man.Id_Manufacturer != ManufacturerId.Value);
                }
                if (alreadyExists)
                {
                    MessageBox.Show("Такой производитель уже существует в базе данных!" +
                        "\nОшибка: название марки или владелец уже используются!");
                    return;
                }
            }

            OnSave?.Invoke(this);
        }

        //Метод отмены изменений
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }
    }
}
