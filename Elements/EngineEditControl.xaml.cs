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
    /// Логика взаимодействия для EngineEditControl.xaml
    /// </summary>
    public partial class EngineEditControl : UserControl
    {
        public event Action<Engine> OnSave;
        public event Action OnCancel;

        public EngineEditControl()
        {
            InitializeComponent();
        }

        public string EngineType
        {
            get => EngineType;
            set => EngineType = value;
        }

        public string EngineCapacity
        {
            get => EngineCapacity;
            set => EngineCapacity = value;
        }

        public string EnginePower
        {
            get => EnginePower;
            set => EnginePower = value;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeBox.Items.Count == 0)
            {
                MessageBox.Show($"Введите тип двигателя");
            }

            if (string.IsNullOrEmpty(CapasityBox.Text))
            {

            }

            if (string.IsNullOrEmpty(PowerBox.Text))
            {

            }

            OnSave?.Invoke(this);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }
    }
}
