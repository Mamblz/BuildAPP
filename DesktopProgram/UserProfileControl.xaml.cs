using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class UserProfileControl : UserControl
    {
        private User _currentUser;

        public event Action BackToMainRequested;

        public UserProfileControl(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            UserNameTextBox.Text = _currentUser.Username;
            EmailTextBox.Text = _currentUser.Email;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackToMainRequested?.Invoke();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = UserNameTextBox.Text.Trim();
            string newEmail = EmailTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(newEmail))
            {
                MessageBox.Show("Введите корректный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentUser.Username = newUsername;
            _currentUser.Email = newEmail;


            StatusTextBlock.Visibility = Visibility.Visible;

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (s, args) =>
            {
                StatusTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            var passwordWindow = new ChangePasswordWindow(_currentUser);
            passwordWindow.ShowDialog();
        }


=======
            MessageBox.Show("Функция смены пароля на данный момент не разработана", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

>>>>>>> 3ac8f3ca4d0745ba92160490b60861e696711a53
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
<<<<<<< HEAD

        private void ChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            var changeEmailWindow = new ChangeEmailWindow(EmailTextBox.Text);
            if (changeEmailWindow.ShowDialog() == true)
            {
                EmailTextBox.Text = changeEmailWindow.NewEmailTextBox.Text;
            }
        }


=======
>>>>>>> 3ac8f3ca4d0745ba92160490b60861e696711a53
    }
}