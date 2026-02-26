using projectGamesPlatformers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projectGamesPlatformers.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistationWindow.xaml
    /// </summary>
    public partial class RegistationWindow : Window
    {
        public RegistationWindow()
        {
            InitializeComponent();
        }
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные
            string fullName = txtFullName.Text.Trim();
            string login = txtRegLogin.Text.Trim();
            string pass = txtRegPassword.Password;
            string confirmPass = txtConfirmPassword.Password;

            // 1. Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Проверка совпадения паролей
            if (pass != confirmPass)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var db = new user15Entities())
                {
                    if (db.Users.Any(u => u.Login == login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    User newUser = new User
                    {
                        FullName = fullName,
                        Login = login,
                        Password = pass
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("Регистрация успешно завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
