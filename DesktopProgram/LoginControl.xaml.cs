using System;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Services;

namespace DesktopProgram.Views
{
    public partial class LoginControl : UserControl
    {
        // Событие для уведомления о переключении на экран регистрации
        public event Action SwitchToRegister;

        // Событие для уведомления об успешном входе
        public event Action LoginSuccessful;

        private readonly AuthService _authService;

        public LoginControl()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (_authService.Login(login, password))
            {
                MessageBox.Show("Успешный вход!");
                LoginSuccessful?.Invoke(); // Оповещаем, что вход прошёл успешно
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