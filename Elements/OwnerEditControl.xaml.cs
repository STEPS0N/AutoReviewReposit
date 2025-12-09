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

            if (string.IsNullOrWhiteSpace(OwnerPhone))
            {
                MessageBox.Show("Введите телефон владельца!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(OwnerEmail, @"^[A-Za-z0-9][A-Za-z0-9._%+-]+@[^@\s]+\.ru+$"))
            {
                MessageBox.Show("Неверный формат email! \n Пример: permaviat@permaviat.ru");
                return;
            }

            if (!string.IsNullOrEmpty(OwnerPhone) && !System.Text.RegularExpressions.Regex.IsMatch(OwnerPhone, @"^\+[1-9]\d{2,11}$"))
            {
                MessageBox.Show("Неверный формат телефона! \n Должно начинаться с '+' и содержать от 3 до 12 цифр. \n Пример: +79128887054");
                return;
            }

            using (var context = new AppDbContext($"Server=WIN-R32OTPM964O\\SQLEXPRESS;Database=AutoReview;User Id={AuthData.Login};Password={AuthData.Password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
            {
                bool alreadyExists = context.Owners.Any(o => o.Fio == OwnerFio || o.Owner_Email == OwnerEmail);

                if (OwnerId.HasValue)
                {
                    alreadyExists = context.Owners.Any(o => o.Fio == OwnerFio || o.Owner_Email == OwnerEmail ||
                    o.Id_owner != OwnerId.Value);
                }
                if (alreadyExists)
                {
                    MessageBox.Show("Такой владелец уже существует в базе данных!");
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
