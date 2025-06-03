using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DesktopProgram.Services;

namespace BuildFlowApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;

        public RegisterViewModel()
        {
            _authService = new AuthService();
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        protected virtual bool RegisterUser(string username, string email, string password)
        {
            return _authService.Register(username, email, password);
        }


        public event Action<string> OnError;
        public event Action OnSuccess;

        public bool Register()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                OnError?.Invoke("Пожалуйста, заполните все поля.");
                return false;
            }

            if (Username.Length < 3)
            {
                OnError?.Invoke("Логин пользователя должен содержать минимум 3 символа.");
                return false;
            }

            if (!IsValidEmail(Email))
            {
                OnError?.Invoke("Пожалуйста, введите корректный email адрес.");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                OnError?.Invoke("Пароли не совпадают.");
                return false;
            }

            if (!IsPasswordStrong(Password))
            {
                OnError?.Invoke("Пароль должен содержать минимум 8 символов, включая цифры и буквы.");
                return false;
            }

            bool success = RegisterUser(Username, Email, Password);
            if (success)
            {
                OnSuccess?.Invoke();
                return true;
            }
            else
            {
                OnError?.Invoke("Пользователь с таким именем или email уже существует.");
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
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
            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, "[a-zA-Z]");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string prop = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
