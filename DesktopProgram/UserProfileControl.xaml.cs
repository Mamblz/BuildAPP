using System;
using System.Windows.Controls;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class UserProfileControl : UserControl
    {
        private User _currentUser;

        // Событие для возврата назад
        public event Action BackToMainRequested;

        // Конструктор с передачей пользователя
        public UserProfileControl(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // Заполняем данные на форме
            UserNameTextBlock.Text = _currentUser.Username;
            EmailTextBlock.Text = _currentUser.Email;
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BackToMainRequested?.Invoke();
        }
    }
}
