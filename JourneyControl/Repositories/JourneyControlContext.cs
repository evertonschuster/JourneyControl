using JourneyControl.Models;
using Microsoft.EntityFrameworkCore;

namespace JourneyControl.Repositories
{
    internal class JourneyControlContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; } 

        private string DbPath { get; }

        public JourneyControlContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Journey.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
