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
                Background = new LinearGradientBrush(
                    Color.FromRgb(255, 255, 255),
                    Color.FromRgb(240, 248, 255),
                    90),
                CornerRadius = new CornerRadius(18),
                BorderBrush = new SolidColorBrush(Color.FromRgb(190, 210, 230)),
                BorderThickness = new Thickness(2),
                Width = 300,
                Height = 310,
                Margin = new Thickness(12),
                Padding = new Thickness(20),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.LightSlateGray,
                    BlurRadius = 15,
                    ShadowDepth = 5,
                    Opacity = 0.25
                },
                Cursor = Cursors.Hand
            };

            border.MouseEnter += (s, e) =>
            {
                var anim = new DoubleAnimation(1.05, TimeSpan.FromMilliseconds(200));
                border.RenderTransformOrigin = new Point(0.5, 0.5);
                border.RenderTransform = new ScaleTransform(1, 1);
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            };
            border.MouseLeave += (s, e) =>
            {
                var anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            };

            var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Stretch };

            var title = new TextBlock
            {
                Text = building.Name,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(22, 35, 52)),
                Margin = new Thickness(0, 0, 0, 12),
                TextAlignment = TextAlignment.Center
            };

            var status = new TextBlock
            {
                Text = $"Статус: {building.Status}",
                FontWeight = FontWeights.Medium,
                Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Margin = new Thickness(0, 0, 0, 8),
                TextAlignment = TextAlignment.Center
            };

            var progressBar = new ProgressBar
            {
                Value = building.Progress,
                Maximum = 100,
                Height = 24,
                Margin = new Thickness(0, 0, 0, 14),
                Foreground = new SolidColorBrush(Color.FromRgb(41, 128, 185))
            };

            var resourcesLabel = new TextBlock
            {
                Text = "Необходимые ресурсы:",
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                Margin = new Thickness(0, 0, 0, 6)
            };

            var resourcesStack = new StackPanel();

            foreach (var br in building.BuildingResources)
            {
                string info = $"{br.Resource.Name} x{br.Quantity} (Стоимость: {br.Resource.Cost})";
                var resText = new TextBlock
                {
                    Text = info,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(95, 95, 95)),
                    ToolTip = $"Стоимость одного: {br.Resource.Cost}"
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

        private void RefreshBuildings_Click(object sender, RoutedEventArgs e)
        {
            LoadBuildings();
        }

        private void AddBuilding_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddBuildingWindow
            {
                Owner = Window.GetWindow(this)
            };

            if (addWindow.ShowDialog() == true)
            {
                var newBuilding = addWindow.NewBuilding;

                _context.Buildings.Add(newBuilding);
                _context.SaveChanges();

                LoadBuildings();
            }
        }

        private void DeleteBuilding_Click(object sender, RoutedEventArgs e)
        {
            string nameToDelete = DeleteBuildingNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nameToDelete))
            {
                MessageBox.Show("Введите название здания для удаления.");
                return;
            }

            var building = _context.Buildings.FirstOrDefault(b => b.Name == nameToDelete);

            if (building == null)
            {
                MessageBox.Show($"Здание с названием '{nameToDelete}' не найдено.");
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить здание '{nameToDelete}'?",
                                         "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var relatedResources = _context.BuildingResources.Where(br => br.BuildingId == building.Id).ToList();
                _context.BuildingResources.RemoveRange(relatedResources);

                _context.Buildings.Remove(building);
                _context.SaveChanges();

                MessageBox.Show($"Здание '{nameToDelete}' успешно удалено.");

                LoadBuildings();

                DeleteBuildingNameTextBox.Text = "";
            }
        }
    }
}
