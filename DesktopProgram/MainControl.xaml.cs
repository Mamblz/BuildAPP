using System;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class MainControl : UserControl
    {
        private User _currentUser;

        public event Action ResourcesRequested;
        public event Action UserProfileRequested;
        public event Action LogoutRequested;
        public event Action HomeRequested;
        public event Action BuildingsRequested;

        private BuildingsControl _buildingsControl;

        public MainControl(User user)
        {
            InitializeComponent();
            _currentUser = user;
            GreetingText.Text = $"Привет, {_currentUser.Username}!";

            LoadProjects();
        }

        private void LoadBuildings_Click(object sender, RoutedEventArgs e)
        {
            BuildingsRequested?.Invoke();

            _buildingsControl = new BuildingsControl();

            _buildingsControl.LoadResourcesRequested += () =>
            {
                ResourcesRequested?.Invoke();
            };

            _buildingsControl.ShowProfileRequested += () =>
            {
                UserProfileRequested?.Invoke();
            };

            _buildingsControl.LogoutRequested += () =>
            {
                LogoutRequested?.Invoke();
            };

            _buildingsControl.GoHomeRequested += () =>
            {
                HomeRequested?.Invoke();
            };

            ContentArea.Content = _buildingsControl;
        }

        private void LoadResources_Click(object sender, RoutedEventArgs e)
        {
            ResourcesRequested?.Invoke();
        }

        private void ShowUserProfile_Click(object sender, RoutedEventArgs e)
        {
            UserProfileRequested?.Invoke();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke();
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            HomeRequested?.Invoke();
        }

        private void LoadProjects()
        {
            var projectsControl = new ProjectsControl();
            ContentArea.Content = projectsControl;
        }

        private void GoToTasks_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Задания пока не реализованы.");
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Обновление данных пока не реализовано.");
        }
    }
}
