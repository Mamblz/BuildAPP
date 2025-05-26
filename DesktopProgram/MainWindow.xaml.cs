using System.Windows;
using BuildFlowApp.Views;
using DesktopProgram.Models;
using DesktopProgram.Views;

namespace DesktopProgram
{
    public partial class MainWindow : Window
    {
        private User _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            ShowLogin();
        }

        private void ShowLogin()
        {
            var loginControl = new LoginControl();
            loginControl.SwitchToRegister += ShowRegister;
            loginControl.LoginSuccessful += OnLoginSuccessful;
            MainContent.Content = loginControl;
        }

        private void ShowRegister()
        {
            var registerControl = new RegisterControl();
            registerControl.SwitchToLogin += ShowLogin;
            MainContent.Content = registerControl;
        }

        private void OnLoginSuccessful(User user)
        {
            _currentUser = user;
            ShowMainControl();
        }

        private void ShowMainControl()
        {
            var mainControl = new MainControl(_currentUser);
            mainControl.UserProfileRequested += ShowUserProfile;
            mainControl.HomeRequested += ShowMainControl;
            mainControl.BuildingsRequested += ShowBuildingsView;
            mainControl.ResourcesRequested += ShowResourcesView;
            mainControl.LogoutRequested += Logout;
            MainContent.Content = mainControl;
        }

        private void ShowUserProfile()
        {
            var userProfileControl = new UserProfileControl(_currentUser);
            userProfileControl.BackToMainRequested += ShowMainControl;
            MainContent.Content = userProfileControl;
        }

        private void ShowBuildingsView()
        {
            var buildingsControl = new BuildingsControl();
            MainContent.Content = buildingsControl;
        }

        private void ShowResourcesView()
        {
            var resourcesControl = new ResourcesControl();
            MainContent.Content = resourcesControl;
        }


        private void Logout()
        {
            var result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _currentUser = null;
                ShowLogin();
            }
        }
    }
}
