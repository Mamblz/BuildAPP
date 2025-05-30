using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopProgram.Data;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class AddBuildingWindow : Window
    {
        private readonly ApplicationDbContext _context;
        public Building NewBuilding { get; private set; }

        public AddBuildingWindow()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            LoadResources();
        }

        private void LoadResources()
        {
            var resources = _context.Resources
                .OrderBy(r => r.Name)
                .ToList();

            ResourceComboBox.ItemsSource = resources;
            ResourceComboBox.DisplayMemberPath = "Name";
            ResourceComboBox.SelectedValuePath = "Id";
        }

        private void AddResource_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите ресурс");
                return;
            }
            if (!int.TryParse(ResourceQuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            var selectedResource = (Resource)ResourceComboBox.SelectedItem;

            var existing = ResourcesDataGrid.Items.Cast<BuildingResource>()
                .FirstOrDefault(br => br.ResourceId == selectedResource.Id);

            if (existing != null)
            {
                existing.Quantity += quantity;
                ResourcesDataGrid.Items.Refresh();
            }
            else
            {
                var newBr = new BuildingResource
                {
                    Resource = selectedResource,
                    ResourceId = selectedResource.Id,
                    Quantity = quantity
                };

                ResourcesDataGrid.Items.Add(newBr);
            }

            ResourceQuantityTextBox.Text = "";
        }

        private void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is BuildingResource br)
            {
                ResourcesDataGrid.Items.Remove(br);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название здания");
                return;
            }

            if (string.IsNullOrWhiteSpace(StatusTextBox.Text))
            {
                MessageBox.Show("Введите статус здания");
                return;
            }

            if (!int.TryParse(ProgressTextBox.Text, out int progress) || progress < 0 || progress > 100)
            {
                MessageBox.Show("Введите корректный прогресс (0-100)");
                return;
            }

            var building = new Building
            {
                Name = NameTextBox.Text.Trim(),
                Status = StatusTextBox.Text.Trim(),
                Progress = progress,
                BuildingResources = new List<BuildingResource>()
            };

            foreach (BuildingResource br in ResourcesDataGrid.Items)
            {
                building.BuildingResources.Add(new BuildingResource
                {
                    ResourceId = br.ResourceId,
                    Quantity = br.Quantity
                });
            }

            _context.Buildings.Add(building);
            _context.SaveChanges();

            NewBuilding = building;
            DialogResult = true;
            Close();
        }
    }
}
