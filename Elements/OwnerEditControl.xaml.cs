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
    /// Логика взаимодействия для UserEditControl.xaml
    /// </summary>
    public partial class OwnerEditControl : UserControl
    {
        public event Action<OwnerEditControl> OnSave;
        public event Action OnCancel;

        public OwnerEditControl()
        {
            InitializeComponent();
        }

        public string OwnerFio
        {
            get => FioBox.Text;
            set => FioBox.Text = value;
        }

        public string OwnerEmail
        {
            get => EmailBox.Text;
            set => EmailBox.Text = value;
        }

        public string OwnerPhone
        {
            get => PhoneBox.Text;
            set => PhoneBox.Text = value;
        }

        public int? OwnerId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OwnerFio))
            {
                MessageBox.Show("Введите ФИО владельца!");
                return;
            }

            if (string.IsNullOrWhiteSpace(OwnerEmail))
            {
                MessageBox.Show("Введите email владельца!");
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
