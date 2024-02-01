using Microsoft.EntityFrameworkCore;

namespace TasksList.Database
{
    public class TasksListDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"TasksListApp.sqlite")}");
        }
    }
}
