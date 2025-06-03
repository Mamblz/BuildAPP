using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DesktopProgram.Data;
using DesktopProgram.Models;
using DesktopProgram.Services;

namespace DesktopProgram.ViewModels
{
    public class ChangePasswordViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly User _user;

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        public ChangePasswordViewModel(User user)
        {
            _user = user;
            _authService = new AuthService();

            SaveCommand = new RelayCommand(_ => ExecuteSave());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void ExecuteSave()
        {
            if (string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Все поля обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Новые пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var loggedUser = _authService.Login(_user.Username, OldPassword);
            if (loggedUser == null)
            {
                MessageBox.Show("Старый пароль неверный.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _user.PasswordHash = _authService.GetHashedPassword(NewPassword);
            _authService.UpdateUserPassword(_user);

            MessageBox.Show("Пароль успешно изменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            RequestClose?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
