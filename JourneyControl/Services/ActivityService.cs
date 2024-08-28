using JourneyControl.Models;
using JourneyControl.Repositories;
using System.Timers;

namespace JourneyControl.Services
{
    internal class ActivityService : IActivityService
    {
        private const int _monitoringInterval = 1 * 60 * 1000;
        private const int _tolerance = 1000;

        public IActivityRepository ActivityRepository { get; }
        public IActivityMonitor ActivityMonitor { get; }

        protected System.Timers.Timer? Timer;

        public ActivityService(IActivityRepository activityRepository, IActivityMonitor activityMonitor)
        {
            ActivityRepository = activityRepository;
            ActivityMonitor = activityMonitor;
        }

        public void StartMonitoring()
        {
            RegistreActivity();
            Timer?.Stop();
            Timer?.Dispose();

            Timer = new System.Timers.Timer(_monitoringInterval);
            Timer.Elapsed += (object? sender, ElapsedEventArgs e) => RegistreActivity();
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void StopMonitoring()
        {
            RegistreActivity();
            Timer?.Stop();
        }

        private void RegistreActivity()
        {
            var lastActivityAt = ActivityMonitor.GetLastActivity();

            var model = new Activity()
            {
                ActivityAt = DateTimeOffset.Now,
                Inactivity = lastActivityAt,
                IsActive = lastActivityAt.ToTimeSpan() < TimeSpan.FromMilliseconds(_monitoringInterval)
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
            var monitoringInterval = TimeSpan.FromMilliseconds(_monitoringInterval + _tolerance);

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

            if (lastActivity?.IsActive ?? false)
            {
                var timeSinceLastActivity = DateTimeOffset.Now - lastActivity.ActivityAt;
                if (timeSinceLastActivity <= monitoringInterval)
                {
                    totalActive += timeSinceLastActivity;
                }
            }

            return (new TimeOnly(totalActive.Ticks), lastActivity?.IsActive ?? false);
        }
    }
}
