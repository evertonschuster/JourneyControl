using JourneyControl.Application.Services;

namespace App.WindowsService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IActivityService _activityService;
        private readonly IConfiguration _configuration;

        public Worker(IActivityService activityService, IConfiguration configuration, ILogger<Worker> logger)
        {
            _activityService = activityService;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
