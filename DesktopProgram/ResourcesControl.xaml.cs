using DesktopProgram.Data;
using DesktopProgram.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Windows;
using System.Windows.Controls;

namespace DesktopProgram.Views
{
    public partial class ResourcesControl : UserControl
    {
        private readonly ApplicationDbContext _context;

        public ResourcesControl()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadResources();
        }

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
    }
}