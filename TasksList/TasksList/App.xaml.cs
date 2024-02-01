using System.Windows;
using TasksList.Database;
using TasksList.Core.Helpers;

namespace TasksList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var database = new TasksListDbContext();
            database.Database.EnsureCreated();
            DatabaseLocator.Database = database;
        }
    }
}
