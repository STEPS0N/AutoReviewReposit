using AutoReview;
using AutoReview.Classes;
using AutoReview.EntityFramework;
using Microsoft.EntityFrameworkCore;
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

namespace AutoReview.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorisation.xaml
    /// </summary>
    public partial class Authorisation : Page
    {
        public MainWindow mainWindow;

        public Authorisation(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void Entry(object sender, RoutedEventArgs e)
        {
            string login = tb_login.Text;
            string password = tb_password.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                using (AppDbContext context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={login};password={password};"))
                {
                    try
                    {
                        context.Database.OpenConnection();

                        using var cmd = context.Database.GetDbConnection().CreateCommand();
                        cmd.CommandText = "SELECT Grant_priv FROM mysql.user WHERE User = @login";

                        var param = cmd.CreateParameter();
                        param.ParameterName = "@login";
                        param.Value = login;
                        cmd.Parameters.Add(param);

                        var result = cmd.ExecuteScalar()?.ToString();

                        if (result == "Y")
                        {
                            AuthData.Rights = true;
                            
                            
                            MessageBox.Show("Здравствуйте админ!");
                            mainWindow.OpenPage(MainWindow.pages.menu);
                        }
                        else if (result == "N")
                        {
                            AuthData.Rights = false;
                            MessageBox.Show("Здравствуйте пользователь!");
                            mainWindow.OpenPage(MainWindow.pages.menu);
                        }

                        AuthData.Login = login;
                        AuthData.Password = password;
                    }
                    finally
                    {
                        context.Database.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: Пользователь не найден! (Неправильный логин или пароль)");
                return;
            }
        }
    }

}

//Server = ISP - 23 - 1 - 7\\KLIM_MILN; Database = AutoReview; User Id = { login }; Password ={ password}
//; Trusted_Connection = False; MultipleActiveResultSets = true; TrustServerCertificate = True;

//private void Entry(object sender, RoutedEventArgs e)
//{
//    string login = tb_login.Text;
//    string password = tb_password.Text;

//    if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
//    {
//        MessageBox.Show("Введите логин и пароль!");
//        return;
//    }

//    try
//    {
//        using (AppDbContext context = new AppDbContext($"Server=ISP-23-1-7\\KLIM_MILN;Database=AutoReview;User Id={login};Password={password};Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
//        {
//            context.Database.OpenConnection();

//            try
//            {
//                using var cmd = context.Database.GetDbConnection().CreateCommand();
//                cmd.CommandText = $"SELECT IS_MEMBER('db_owner')";
//                int result = (int)cmd.ExecuteScalar();

//                if (result == 1)
//                {
//                    MessageBox.Show("Здравствуйте админ!");
//                    mainWindow.OpenPage(MainWindow.pages.menu);
//                }
//                else
//                {
//                    AuthData.Rights = false;
//                    MessageBox.Show("Здравствуйте пользователь!");
//                    mainWindow.OpenPage(MainWindow.pages.menu);
//                }

//                AuthData.Login = login;
//                AuthData.Password = password;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка: {ex.Message}\nПроверьте логин и пароль");
//            }
//            finally
//            {
//                context.Database.CloseConnection();
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show($"Ошибка: Пользователь не найден! (Неправильный логин или пароль)");
//        return;
//    }

//}


//private void Entry(object sender, RoutedEventArgs e)
//{
//    string login = tb_login.Text;
//    string password = tb_password.Password;

//    using (AppDbContext context = new AppDbContext($"server=localhost;port=3307;database=AutoReview;user={login};password={password};"))
//    {
//        context.Database.OpenConnection();

//        try
//        {
//            using var cmd = context.Database.GetDbConnection().CreateCommand();
//            cmd.CommandText = "SELECT Grant_priv FROM mysql.user WHERE User = @login";

//            var param = cmd.CreateParameter();
//            param.ParameterName = "@login";
//            param.Value = login;
//            cmd.Parameters.Add(param);

//            var result = cmd.ExecuteScalar()?.ToString();

//            if (result == "Y")
//            {
//                MessageBox.Show("Здравствуйте админ!");
//                mainWindow.OpenPage(MainWindow.pages.menu);
//            }
//            else if (result == "N")
//            {
//                MessageBox.Show("Здравствуйте пользователь!");
//                mainWindow.OpenPage(MainWindow.pages.menu);
//            }
//            else
//            {
//                MessageBox.Show("Пользователь не найден в mysql.user");
//            }

//            AuthData.Login = login;
//            AuthData.Password = password;
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show($"Ошибка: {ex.Message}\nПроверьте логин и пароль");
//        }
//        finally
//        {
//            context.Database.CloseConnection();
//        }
//    }
//}