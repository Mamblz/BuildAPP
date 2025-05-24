using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            BuildingsPanel.Children.Clear();

            var buildings = _context.Buildings
                .Include(b => b.BuildingResources)
                .ThenInclude(br => br.Resource)
                .ToList();

            foreach (var b in buildings)
            {
                BuildingsPanel.Children.Add(CreateBuildingCard(b));
            }

            // Дополнительные примеры новых построек, если хочешь добавить вручную:
            BuildingsPanel.Children.Add(CreateBuildingCard(new Building
            {
                Name = "Парк",
                Status = "Строится",
                Progress = 45,
                BuildingResources = new System.Collections.Generic.List<BuildingResource>
                {
                    new BuildingResource { Resource = new Resource { Name = "Дерево", Cost = 10 }, Quantity = 30 },
                    new BuildingResource { Resource = new Resource { Name = "Камень", Cost = 15 }, Quantity = 20 }
                }
            }));

            BuildingsPanel.Children.Add(CreateBuildingCard(new Building
            {
                Name = "Больница",
                Status = "Готово",
                Progress = 100,
                BuildingResources = new System.Collections.Generic.List<BuildingResource>
                {
                    new BuildingResource { Resource = new Resource { Name = "Металл", Cost = 50 }, Quantity = 40 },
                    new BuildingResource { Resource = new Resource { Name = "Стекло", Cost = 30 }, Quantity = 25 }
                }
            }));
        }

        private Border CreateBuildingCard(Building building)
        {
            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(15),
                BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                BorderThickness = new Thickness(1),
                Width = 280,
                Height = 240,
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Gray,
                    BlurRadius = 8,
                    ShadowDepth = 3,
                    Opacity = 0.2
                }
            };

            var stack = new StackPanel();

            var title = new TextBlock
            {
                Text = building.Name,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(34, 34, 34)),
                Margin = new Thickness(0, 0, 0, 8),
                TextAlignment = TextAlignment.Center
            };

            var status = new TextBlock
            {
                Text = $"Статус: {building.Status}",
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(85, 85, 85)),
                Margin = new Thickness(0, 0, 0, 6),
                TextAlignment = TextAlignment.Center
            };

            var progressBar = new ProgressBar
            {
                Value = building.Progress,
                Maximum = 100,
                Height = 22,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var resourcesLabel = new TextBlock
            {
                Text = "Необходимые ресурсы:",
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                Margin = new Thickness(0, 0, 0, 5)
            };

            var resourcesStack = new StackPanel();

            foreach (var br in building.BuildingResources)
            {
                string info = $"{br.Resource.Name} x{br.Quantity} (Стоимость: {br.Resource.Cost})";
                var resText = new TextBlock
                {
                    Text = info,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120))
                };
                resourcesStack.Children.Add(resText);
            }

            stack.Children.Add(title);
            stack.Children.Add(status);
            stack.Children.Add(progressBar);
            stack.Children.Add(resourcesLabel);
            stack.Children.Add(resourcesStack);

            border.Child = stack;

            return border;
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            UserProfileRequested?.Invoke();
        }
    }
}
