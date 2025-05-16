using System.Windows;
using BuildFlowApp.Services;
using BuildFlowApp.Data;

namespace BuildFlowApp
{
    public partial class MainWindow : Window
    {
        private readonly AuthService _authService;

        public MainWindow()
        {
            InitializeComponent();
            _authService = new AuthService(new ApplicationDbContext());

            UpdatePasswordPlaceholder();
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user = _authService.Login(LoginTextBox.Text, PasswordBox.Password);
            ShowMessage(user != null ? $"Добро пожаловать, {user.Username}" : "Неверные данные");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = _authService.Register("User", LoginTextBox.Text, PasswordBox.Password);
            ShowMessage(success ? "Регистрация успешна!" : "Пользователь уже существует");
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordPlaceholder();
        }

        private void UpdatePasswordPlaceholder()
        {
            PasswordPlaceholder.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
