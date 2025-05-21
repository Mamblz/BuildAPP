using DesktopProgram.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace BuildFlowApp.Views
{
    public partial class MainControl : UserControl
    {
        private readonly ApplicationDbContext _context;

        public MainControl()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            var buildings = _context.Buildings
                .Include(b => b.BuildingResources)
                .ThenInclude(br => br.Resource)
                .ToList();

            var displayList = buildings.Select(b => new
            {
                b.Name,
                b.Status,
                b.Progress,
                BuildingResources = b.BuildingResources.Select(br => new
                {
                    ResourceInfo = $"{br.Resource.Name} x{br.Quantity} (Стоимость: {br.Resource.Cost})"
                }).ToList()
            }).ToList();

            BuildingsList.ItemsSource = displayList;
        }
    }
}
