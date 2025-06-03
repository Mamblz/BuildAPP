using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DesktopProgram.Models;
using DesktopProgram.Services;

namespace DesktopProgram.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<User> LoginSuccessful;
        public event Action SwitchToRegister;

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new RelayCommand(_ => ExecuteLogin());
            RegisterCommand = new RelayCommand(_ => SwitchToRegister?.Invoke());
        }

        private void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private bool IsPasswordStrong(string password)
        {
            return password?.Length >= 8 &&
                   Regex.IsMatch(password, @"\d") &&
                   Regex.IsMatch(password, @"[a-zA-Z]");
        }

        public bool IsInputValid(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            if (Login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа.");
                return false;
            }

            if (!IsPasswordStrong(Password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, включая цифры и буквы.");
                return false;
            }

            return true;
        }

        private void ExecuteLogin()
        {
            if (!IsInputValid(Login, Password))
                return;

            var user = _authService.Login(Login, Password);

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
    }
}
