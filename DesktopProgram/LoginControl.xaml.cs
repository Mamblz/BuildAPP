using System;
using System.Windows;
using System.Windows.Controls;
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

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
