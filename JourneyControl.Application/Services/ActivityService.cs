using JourneyControl.Application.Models;
using JourneyControl.Application.Repositories;
using Microsoft.Extensions.Configuration;

namespace JourneyControl.Application.Services
{
    internal class ActivityService : IActivityService
    {
        private const int _tolerance = 1000;
        private readonly int MonitoringInterval;

        public IActivityRepository ActivityRepository { get; }
        public IActivityMonitor ActivityMonitor { get; }

        public ActivityService(IActivityRepository activityRepository, IActivityMonitor activityMonitor, IConfiguration configuration)
        {
            ActivityRepository = activityRepository;
            ActivityMonitor = activityMonitor;

            var timeInSecounds = configuration["MonitoringInterval"] ?? "60";
            MonitoringInterval = int.Parse(timeInSecounds) * 1000;
        }

        public void RegisterActivity()
        {
            var lastActivityAt = ActivityMonitor.GetLastActivity();

            var model = new Activity()
            {
                ActivityAt = DateTimeOffset.Now,
                Inactivity = lastActivityAt,
                IsActive = lastActivityAt.ToTimeSpan() < TimeSpan.FromMilliseconds(MonitoringInterval)
            };

            ActivityRepository.Save(model);
        }

        public (TimeOnly time, bool isActive) GetTodayActivity()
        {
            var activities = ActivityRepository.GetDateActivity(DateTime.UtcNow)
                .OrderBy(e => e.ActivityAt)
                .ToArray();

            if (activities.Length == 0)
            {
                return (new TimeOnly(0), false);
            }

            var totalActive = TimeSpan.Zero;
            var monitoringInterval = TimeSpan.FromMilliseconds(MonitoringInterval + _tolerance);

            for (int i = 1; i < activities.Length; i++)
            {
                if (activities[i].IsActive)
                {
                    var timeDifference = activities[i].ActivityAt - activities[i - 1].ActivityAt;
                    if (timeDifference <= monitoringInterval)
                    {
                        totalActive += timeDifference;
                    }
                }
            }

            var lastActivity = activities.LastOrDefault();

            var isActive = false;
            if (lastActivity?.IsActive ?? false)
            {
                var timeSinceLastActivity = DateTimeOffset.Now - lastActivity.ActivityAt;
                if (timeSinceLastActivity <= monitoringInterval)
                {
                    isActive = true;
                    totalActive += timeSinceLastActivity;
                }
            }

            return (new TimeOnly(totalActive.Ticks), isActive);
        }
    }
}
