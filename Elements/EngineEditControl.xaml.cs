using AutoReview.Classes;
using AutoReview.EntityFramework;
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
    /// Логика взаимодействия для EngineEditControl.xaml
    /// </summary>
    public partial class EngineEditControl : UserControl
    {
        public event Action<EngineEditControl> OnSave;
        public event Action OnCancel;

        public EngineEditControl()
        {
            InitializeComponent();
        }

        public string EngineType
        {
            get => TypeBox.Text;
            set => TypeBox.Text = value;
        }

        public string EngineCapacity
        {
            get => CapacityBox.Text;
            set => CapacityBox.Text = value;
        }

        public string EnginePower
        {
            get => PowerBox.Text;
            set => PowerBox.Text = value;
        }

        public int? EngineId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EngineType))
            {
                MessageBox.Show($"Введите тип двигателя");
                return;
            }

            if (string.IsNullOrEmpty(EngineCapacity) || !decimal.TryParse(EngineCapacity, out decimal capacity) || capacity <= 0 || !System.Text.RegularExpressions.Regex.IsMatch(EngineCapacity, @"^\d{1,2}\,\d$"))
            {
                MessageBox.Show("Введите объем двигателя! (Пример: 2.0)");
                return;
            }

            if (string.IsNullOrEmpty(EnginePower) || !int.TryParse(EnginePower, out int power) || power <= 1 || power >= 2000 || !System.Text.RegularExpressions.Regex.IsMatch(EnginePower, @"^\d{1,4}$"))
            {
                MessageBox.Show("Введите мощность двигателя! (Пример: 150. От 1 до 2000)");
                return;
            }

            using (var context = new AppDbContext($"Server=WIN-R32OTPM964O\\SQLEXPRESS;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
            {
                bool alreadyExists = context.Engine.Any(eng => eng.Capacity_Engine == capacity &&
                eng.Power_Engine == power);

                if (EngineId.HasValue)
                {
                    alreadyExists = context.Engine.Any(eng => eng.Capacity_Engine == capacity && eng.Power_Engine == power 
                    && eng.Id_Engine != EngineId.Value);
                }

                if (alreadyExists)
                {
                    MessageBox.Show("Такой двигатель уже существует в базе данных!");
                    return;
                }
            }

            OnSave?.Invoke(this);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }
    }
}
