using JourneyControl.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace JourneyControl.Infra.DB
{
    internal class JourneyControlContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }

        private string DbPath { get; }

        public JourneyControlContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Combine(path, "JourneyControl/Journey.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}