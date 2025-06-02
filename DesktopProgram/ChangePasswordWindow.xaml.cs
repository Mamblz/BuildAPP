using DesktopProgram.Models;
using DesktopProgram.Services;
using System.Windows;

namespace DesktopProgram.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly User _user;
        private readonly AuthService _authService;

        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            _user = user;
            _authService = new AuthService();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var oldPassword = OldPasswordBox.Password;
            var newPassword = NewPasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Все поля обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Новые пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var loggedUser = _authService.Login(_user.Username, oldPassword);
            if (loggedUser == null)
            {
                MessageBox.Show("Старый пароль неверный.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _user.PasswordHash = _authService.GetHashedPassword(newPassword);
            _authService.UpdateUserPassword(_user);

            MessageBox.Show("Пароль успешно изменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
