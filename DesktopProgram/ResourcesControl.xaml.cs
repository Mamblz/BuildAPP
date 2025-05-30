using DesktopProgram.Data;
using DesktopProgram.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopProgram.Views
{
    public partial class ResourcesControl : UserControl
    {
        private readonly ApplicationDbContext _context;
        public event Action GoHomeRequested;


        public ResourcesControl()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadResources();
        }

        public event Action LoadHomeRequested;
        public event Action LoadBuildingsRequested;
        public event Action LoadResourcesRequested;
        public event Action ShowProfileRequested;
        public event Action LogoutRequested;

        private void LoadResources()
        {
            var resources = _context.Resources.ToList();
            ResourcesList.ItemsSource = resources;
        }

        private void AddResource_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            bool costParsed = decimal.TryParse(CostTextBox.Text, out decimal cost);

            if (string.IsNullOrWhiteSpace(name) || !costParsed)
            {
                MessageBox.Show("Проверьте правильность ввода данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newResource = new Resource
            {
                Name = name,
                Cost = (int)Math.Round(cost)
            };

            _context.Resources.Add(newResource);
            _context.SaveChanges();

            ClearInputFields();
            LoadResources();
        }

        private void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Resource resource)
            {
                var result = MessageBox.Show($"Удалить ресурс '{resource.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Resources.Remove(resource);
                    _context.SaveChanges();
                    LoadResources();
                }
            }
        }

        private void ClearInputFields()
        {
            NameTextBox.Text = "";
            CostTextBox.Text = "";
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
