using System;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Services;

namespace DesktopProgram.Views
{
    public partial class LoginControl : UserControl
    {
        public event Action SwitchToRegister;

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