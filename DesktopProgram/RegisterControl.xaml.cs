using System;
using System.Windows;
using System.Windows.Controls;
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

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