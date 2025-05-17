using System.Linq;
using System.Windows;


namespace BuildAPP
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService;

        public RegisterWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
{
                MessageBox.Show("Заполните все пoля.");
                return;
            }

            bool success = _authService.Register(username, email, password);

            if (success)
            {
                MessageBox.Show("Регистрация успешна!");
                Close();
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем или email уже существует.");
            }
        }
    }
}