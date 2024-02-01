using TasksList.Database;

namespace TasksList.Core.Helpers
{
    public class DatabaseLocator
    {
        public static TasksListDbContext Database {get; set;}
    }
}
