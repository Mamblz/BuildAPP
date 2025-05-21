using System.Windows;
using BuildFlowApp.Views;     // Для MainControl
using DesktopProgram.Views;   // Для LoginControl и RegisterControl

namespace BuildFlowApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowLogin();
        }

        public void ShowLogin()
        {
            var loginControl = new LoginControl();
            loginControl.SwitchToRegister += ShowRegister;
            loginControl.LoginSuccessful += ShowMainControl;  // Подписываемся на успешный вход
            MainContent.Content = loginControl;
        }

        public void ShowRegister()
        {
            var registerControl = new RegisterControl();
            registerControl.SwitchToLogin += ShowLogin;
            MainContent.Content = registerControl;
        }

        public void ShowMainControl()
        {
            var mainControl = new MainControl();
            MainContent.Content = mainControl;
        }
    }
}
