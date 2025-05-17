using System.Windows;
using BuildAPP;
using DesktopProgram.Data;
using DesktopProgram.Models;

namespace BuildFlowApp
{
    public partial class MainWindow : Window
    {
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Логин нажат");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }

    }
}
