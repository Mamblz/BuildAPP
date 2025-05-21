using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using DesktopProgram.Services;

namespace BuildFlowApp.Views
{
    public partial class RegisterControl : UserControl
    {
        private readonly AuthService _authService;

        public Action SwitchToLogin { get; internal set; }

        public RegisterControl()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchToLogin?.Invoke();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Используем простую регулярку для проверки email
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsPasswordStrong(string password)
        {
            // Минимум 8 символов, хотя бы одна цифра и одна буква
            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, "[a-zA-Z]");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка имени пользователя
            if (username.Length < 3)
            {
                MessageBox.Show("Имя пользователя должно содержать минимум 3 символа.");
                return;
            }

            // Проверка email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный email адрес.");
                return;
            }

            // Проверка пароля
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            if (!IsPasswordStrong(password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, включая цифры и буквы.");
                return;
            }

            // Регистрация
            bool success = _authService.Register(username, email, password);
            if (success)
            {
                MessageBox.Show("Регистрация успешна! Теперь вы можете войти.");
                SwitchToLogin?.Invoke();
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем или email уже существует.");
            }
        }
    }
}