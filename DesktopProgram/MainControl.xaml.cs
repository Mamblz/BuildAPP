using System;
using System.Linq;
using System.Windows.Controls;
using DesktopProgram.Models;
using DesktopProgram.Data;
using Microsoft.EntityFrameworkCore;

namespace DesktopProgram.Views
{
    public partial class MainControl : UserControl
    {
        private readonly User _currentUser;
        private readonly ApplicationDbContext _context;

        public event Action UserProfileRequested;

        public MainControl(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new ApplicationDbContext();

            LoadBuildings();
        }

        private void LoadBuildings()
        {
            var buildings = _context.Buildings
                .Include(b => b.BuildingResources)
                .ThenInclude(br => br.Resource)
                .ToList();

            var displayList = buildings.Select(b => new BuildingDisplayModel
            {
                Name = b.Name,
                Status = b.Status,
                Progress = b.Progress,
                BuildingResources = b.BuildingResources.Select(br => new ResourceDisplayModel
                {
                    ResourceInfo = $"{br.Resource.Name} x{br.Quantity} (Стоимость: {br.Resource.Cost})"
                }).ToList()
            }).ToList();

            BuildingsList.ItemsSource = displayList;
        }

        private void UserProfileButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserProfileRequested?.Invoke();
        }
    }
}
