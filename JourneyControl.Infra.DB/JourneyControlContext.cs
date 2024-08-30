using JourneyControl.Application.Models;
using JourneyControl.Infra.DB.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JourneyControl.Infra.DB
{
    public class JourneyControlContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }

        private string DbPath { get; }

        public JourneyControlContext(ILogger<JourneyControlContext> logger, IConfiguration configuration)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var dbPath = configuration["dbPath"] ?? Path.Combine(path, "JourneyControl");

            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            logger.LogInformation($"Arquivo de banco de dados configurado em: '{dbPath}'. {path}");
            DbPath = Path.Combine(dbPath, "Journey.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}