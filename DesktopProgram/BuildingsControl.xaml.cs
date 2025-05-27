using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.EntityFrameworkCore;
using DesktopProgram.Data;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class BuildingsControl : UserControl
    {
        private readonly ApplicationDbContext _context;

        public BuildingsControl()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadBuildings();
        }

        public event Action LoadResourcesRequested;
        public event Action ShowProfileRequested;
        public event Action LogoutRequested;

        private void LoadBuildings()
        {
            BuildingsPanel.Children.Clear();

            var buildings = _context.Buildings
                .Include(b => b.BuildingResources)
                .ThenInclude(br => br.Resource)
                .OrderByDescending(b => b.Id)
                .ToList();

            foreach (var b in buildings)
            {
                BuildingsPanel.Children.Add(CreateBuildingCard(b));
            }
        }

        private Border CreateBuildingCard(Building building)
        {
            var border = new Border
            {
                Background = new LinearGradientBrush(Color.FromRgb(255, 255, 255), Color.FromRgb(240, 248, 255), 90),
                CornerRadius = new CornerRadius(18),
                BorderBrush = new SolidColorBrush(Color.FromRgb(190, 210, 230)),
                BorderThickness = new Thickness(2),
                Width = 300,
                Height = 370,
                Margin = new Thickness(12),
                Padding = new Thickness(20),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.LightSlateGray,
                    BlurRadius = 15,
                    ShadowDepth = 5,
                    Opacity = 0.25
                },
                Cursor = Cursors.Arrow
            };

            var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Stretch };

            stack.Children.Add(new TextBlock
            {
                Text = building.Name,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(22, 35, 52)),
                Margin = new Thickness(0, 0, 0, 12),
                TextAlignment = TextAlignment.Center
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"Статус: {building.Status}",
                FontWeight = FontWeights.Medium,
                Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Margin = new Thickness(0, 0, 0, 8),
                TextAlignment = TextAlignment.Center
            });

            stack.Children.Add(new ProgressBar
            {
                Value = building.Progress,
                Maximum = 100,
                Height = 24,
                Margin = new Thickness(0, 0, 0, 14),
                Foreground = new SolidColorBrush(Color.FromRgb(41, 128, 185))
            });

            stack.Children.Add(new TextBlock
            {
                Text = "Необходимые ресурсы:",
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                Margin = new Thickness(0, 0, 0, 6)
            });

            var resourcesStack = new StackPanel();
            foreach (var br in building.BuildingResources)
            {
                resourcesStack.Children.Add(new TextBlock
                {
                    Text = $"{br.Resource.Name} x{br.Quantity} (Стоимость: {br.Resource.Cost})",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(95, 95, 95)),
                    ToolTip = $"Стоимость одного: {br.Resource.Cost}"
                });
            }
            stack.Children.Add(resourcesStack);

            // Кнопка удаления
            var deleteButton = new Button
            {
                Content = "Удалить",
                Background = new SolidColorBrush(Color.FromRgb(200, 50, 50)),
                Foreground = Brushes.White,
                Margin = new Thickness(0, 12, 0, 0),
                Padding = new Thickness(6),
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 100
            };

            deleteButton.Click += (s, e) =>
            {
                var result = MessageBox.Show($"Удалить здание \"{building.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Buildings.Remove(building);
                    _context.SaveChanges();
                    LoadBuildings();
                }
            };

            stack.Children.Add(deleteButton);
            border.Child = stack;
            return border;
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

        private void LoadBuildings_Click(object sender, RoutedEventArgs e)
        {
            LoadBuildings();
        }

        private void AddBuilding_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddBuildingWindow();
            if (window.ShowDialog() == true)
            {
                LoadBuildings();
            }
        }
    }
}
