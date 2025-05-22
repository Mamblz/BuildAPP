using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using DesktopProgram.Models;
using DesktopProgram.Services;

namespace DesktopProgram.Views
{
    public partial class LoginControl : UserControl
    {
        public event Action SwitchToRegister;
        public event Action<User> LoginSuccessful;

        private readonly AuthService _authService;

        public LoginControl()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private bool IsPasswordStrong(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"\d") && Regex.IsMatch(password, @"[a-zA-Z]");
        }

        private bool IsInputValid(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа.");
                return false;
            }

            if (!IsPasswordStrong(password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, включая цифры и буквы.");
                return false;
            }

            return true;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (!IsInputValid(login, password))
                return;

            User user = _authService.Login(login, password);

            if (user != null)
            {
                MessageBox.Show("Успешный вход!");
                LoginSuccessful?.Invoke(user);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchToRegister?.Invoke();
        }
    }
}
