using System;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Models;
using DesktopProgram.Services;
using DesktopProgram.ViewModels;

namespace DesktopProgram.Views
{
    public partial class LoginControl : UserControl
    {
        private readonly LoginViewModel _viewModel;

        // Публичные события с нужными именами
        public event Action<User> LoginSuccessful;
        public event Action SwitchToRegister;

        public LoginControl()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel(new AuthService());
            DataContext = _viewModel;

            _viewModel.LoginSuccessful += OnLoginSuccessful;
            _viewModel.SwitchToRegister += OnSwitchToRegister;

            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }

        private void OnLoginSuccessful(User user)
        {
            LoginSuccessful?.Invoke(user);
        }

        private void OnSwitchToRegister()
        {
            SwitchToRegister?.Invoke();
        }
    }
}
