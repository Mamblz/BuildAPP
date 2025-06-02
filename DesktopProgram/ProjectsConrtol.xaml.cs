using System.Linq;
using System.Windows.Controls;
using DesktopProgram.Data;
using DesktopProgram.Models;

namespace DesktopProgram.Views
{
    public partial class ProjectsControl : UserControl
    {
        public ProjectsControl()
        {
            InitializeComponent();
            LoadProjectsFromDatabase();
        }

        private void LoadProjectsFromDatabase()
        {
            using (var context = new ApplicationDbContext())
            {
                var projects = context.Projects.ToList();
                ProjectsList.ItemsSource = projects;
            }
        }
    }
}
