using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopProgram.Models;
using DesktopProgram.Data;

namespace DesktopProgram.Views
{
    public partial class MainControl : UserControl
    {
        private readonly User _currentUser;
        private readonly ApplicationDbContext _context;

        private BuildingsControl _buildingsControl;
        private ResourcesControl _resourcesControl;

        public event Action UserProfileRequested;
        public event Action LogoutRequested;
        public event Action HomeRequested;
        public event Action BuildingsRequested;
        public event Action ResourcesRequested;
        public event Action TasksRequested;

        public MainControl(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new ApplicationDbContext();

            _buildingsControl = new BuildingsControl();
            _resourcesControl = new ResourcesControl();

            GreetingText.Text = $"Добро пожаловать, {_currentUser.Username}! Сегодня {DateTime.Now:dd MMMM yyyy}.";

            LoadStats();
        }

        private void LoadStats()
        {
            int buildingsCount = _context.Buildings.Count();
            var goldResource = _context.Resources.FirstOrDefault(r => r.Name == "Gold");
            var woodResource = _context.Resources.FirstOrDefault(r => r.Name == "Дерево");
            var stoneResource = _context.Resources.FirstOrDefault(r => r.Name == "Камень");

            int goldCount = 0, woodCount = 0, stoneCount = 0;

            if (goldResource != null)
            {
                goldCount = _context.BuildingResources
                    .Where(br => br.ResourceId == goldResource.Id)
                    .Sum(br => (int?)br.Quantity) ?? 0;
            }
            if (woodResource != null)
            {
                woodCount = _context.BuildingResources
                    .Where(br => br.ResourceId == woodResource.Id)
                    .Sum(br => (int?)br.Quantity) ?? 0;
            }
            if (stoneResource != null)
            {
                stoneCount = _context.BuildingResources
                    .Where(br => br.ResourceId == stoneResource.Id)
                    .Sum(br => (int?)br.Quantity) ?? 0;
            }

            GoldText.Text = goldCount.ToString();
            WoodText.Text = woodCount.ToString();
            StoneText.Text = stoneCount.ToString();

            int activeTasks = 3;

            StatsText.Text = $"Количество построек: {buildingsCount}\n" +
                             $"Активных заданий: {activeTasks}";
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            HomeRequested?.Invoke();
        }

        private void LoadBuildings_Click(object sender, RoutedEventArgs e)
        {
            BuildingsRequested?.Invoke();
        }

        private void LoadResources_Click(object sender, RoutedEventArgs e)
        {
            ResourcesRequested?.Invoke();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke();
        }

        private void ShowUserProfile_Click(object sender, RoutedEventArgs e)
        {
            UserProfileRequested?.Invoke();
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadStats();
            MessageBox.Show("Данные обновлены");
        }

        private void GoToTasks_Click(object sender, RoutedEventArgs e)
        {
            TasksRequested?.Invoke();
        }
    }
}
