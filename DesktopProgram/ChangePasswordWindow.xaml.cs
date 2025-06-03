using System.Windows;
using DesktopProgram.Models;
using DesktopProgram.ViewModels;

namespace DesktopProgram.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly ChangePasswordViewModel _viewModel;

        public ChangePasswordWindow(User user)
        {
            InitializeComponent();

            _viewModel = new ChangePasswordViewModel(user);
            DataContext = _viewModel;

            _viewModel.RequestClose += () => this.Close();
        }

        private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.OldPassword = OldPasswordBox.Password;
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.NewPassword = NewPasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
}
