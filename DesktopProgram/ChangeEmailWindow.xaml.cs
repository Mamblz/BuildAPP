using System.Windows;

namespace DesktopProgram.Views
{
    public partial class ChangeEmailWindow : Window
    {
        public ChangeEmailWindow(string currentEmail)
        {
            InitializeComponent();
            CurrentEmailTextBox.Text = currentEmail;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = NewEmailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
            {
                MessageBox.Show("Введите корректный Email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void NewEmailTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            PlaceholderTextBlock.Visibility = string.IsNullOrWhiteSpace(NewEmailTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

    }
}
