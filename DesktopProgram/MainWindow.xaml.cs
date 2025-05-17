using System;
using System.Windows;
using BuildFlowApp.Views;
using DesktopProgram.Views;

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
            MainContent.Content = loginControl;
        }

        public void ShowRegister()
        {
            var registerControl = new RegisterControl();
            registerControl.SwitchToLogin += ShowLogin;
            MainContent.Content = registerControl;
        }
    }
}