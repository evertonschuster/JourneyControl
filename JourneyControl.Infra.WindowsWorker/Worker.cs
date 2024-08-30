using JourneyControl.Application.Services;
using JourneyControl.Infra.DB;
using Microsoft.EntityFrameworkCore;

namespace App.WindowsService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IActivityService _activityService;
        private readonly IConfiguration _configuration;
        private readonly JourneyControlContext _dbContext;

        public Worker(IActivityService activityService, IConfiguration configuration, JourneyControlContext dbContext, ILogger<Worker> logger)
        {
            _activityService = activityService;
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _dbContext.Database.Migrate();

            var timeInSecounds = _configuration["MonitoringInterval"] ?? "60";
            var delay = int.Parse(timeInSecounds) * 1000;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _activityService.RegisterActivity();

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
