using DesktopProgram.ViewModels;
using DesktopProgram.Models;
using DesktopProgram.Data;
using System.Windows;

namespace DesktopProgram.Views
{
    public partial class AddBuildingWindow : Window
    {
        private readonly AddBuildingViewModel _viewModel;

        public AddBuildingWindow()
        {
            InitializeComponent();
            var context = new ApplicationDbContext();
            var service = new BuildingService(context);
            _viewModel = new AddBuildingViewModel(service);
            DataContext = _viewModel;
        }

        internal void AddResource_Click(object sender, RoutedEventArgs e)
        {
            var error = _viewModel.AddResource();
            if (error != null)
            {
                MessageBox.Show(error);
            }
        }

        internal void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is BuildingResource br)
            {
                _viewModel.RemoveResource(br);
            }
        }

        internal void Add_Click(object sender, RoutedEventArgs e)
        {
            var error = _viewModel.ValidateAndCreateBuilding();
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            DialogResult = true;
            Close();
        }

        internal void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public Building NewBuilding => _viewModel.CreatedBuilding;
    }
}
