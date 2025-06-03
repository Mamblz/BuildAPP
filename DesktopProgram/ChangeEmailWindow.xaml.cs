using DesktopProgram.ViewModels;
using System.Windows;

namespace DesktopProgram.Views
{
    public partial class ChangeEmailWindow : Window
    {
        private readonly ChangeEmailViewModel _viewModel;

        public ChangeEmailWindow(string currentEmail)
        {
            InitializeComponent();
            _viewModel = new ChangeEmailViewModel(currentEmail);
            DataContext = _viewModel;

            _viewModel.RequestClose += () =>
            {
                DialogResult = _viewModel.DialogResult;
                Close();
            };
        }

        public string NewEmail => _viewModel.NewEmail;
    }
}
