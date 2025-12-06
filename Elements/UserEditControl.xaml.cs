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
    public partial class UserEditControl : UserControl
    {
        public event Action<UserEditControl> OnSave;
        public event Action OnCancel;

        public UserEditControl()
        {
            InitializeComponent();
        }

        public string UserLogin
        {
            get => LoginBox.Text;
            set => LoginBox.Text = value;
        }

        public string UserPassword
        {
            get => PasswordBox.Password;
            set => PasswordBox.Password = value;
        }

        public string UserEmail
        {
            get => EmailBox.Text;
            set => EmailBox.Text = value;
        }

        public int? UserId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserLogin))
            {
                MessageBox.Show("Введите логин пользователя!");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserEmail))
            {
                MessageBox.Show("Введите email пользователя!");
                return;
            }

            if (UserId == null || string.IsNullOrWhiteSpace(UserPassword))
            {
                MessageBox.Show("Введите пароль пользователя!");
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
