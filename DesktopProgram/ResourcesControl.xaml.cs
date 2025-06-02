using DesktopProgram.Data;
using DesktopProgram.Models;
using DesktopProgram.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DesktopProgram.Views
{
    public partial class ResourcesControl : UserControl
    {
        private readonly ResourcesViewModel _viewModel;

        public event Action GoHomeRequested;
        public event Action LoadHomeRequested;
        public event Action LoadBuildingsRequested;
        public event Action LoadResourcesRequested;
        public event Action ShowProfileRequested;
        public event Action LogoutRequested;

        public ResourcesControl()
        {
            InitializeComponent();
            _viewModel = new ResourcesViewModel(new ApplicationDbContext());
            DataContext = _viewModel;
        }

        private void AddResource_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddResource();
        }

        private void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Resource resource)
            {
                _viewModel.DeleteResource(resource);
            }
        }

        private void LoadHome_Click(object sender, RoutedEventArgs e)
        {
            GoHomeRequested?.Invoke();
        }

        private void LoadBuildings_Click(object sender, RoutedEventArgs e)
        {
            LoadBuildingsRequested?.Invoke();
        }

        private void LoadResources_Click(object sender, RoutedEventArgs e)
        {
            LoadResourcesRequested?.Invoke();
        }

        private void ShowUserProfile_Click(object sender, RoutedEventArgs e)
        {
            ShowProfileRequested?.Invoke();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke();
        }
    }
}
