using JourneyControl.Models;
using JourneyControl.Repositories;
using System.Timers;

namespace JourneyControl.Services
{
    internal class ActivityService : IActivityService
    {
        private const int _monitoringInterval = 1 * 60 * 1000;

        public IActivityRepository ActivityRepository { get; }
        public IActivityMonitor ActivityMonitor { get; }

        protected System.Timers.Timer? Timer;
        private readonly List<Action<Activity>> _events = new List<Action<Activity>>();

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
            foreach (var item in _events)
            {
                item(model);
            }
        }

        public void OnChange(Action<Activity> @event)
        {
            _events.Add(@event);
        }

        public TimeOnly GetTodayActivity()
        {
            var activity = ActivityRepository.GetDateActivity(DateTime.UtcNow)
                .OrderBy(e => e.ActivityAt)
                .ToArray();
            var totalActive = TimeSpan.Zero;

            for (int i = 1; i < activity.Count(); i++)
            {
                var before = activity[i - 1];
                var current = activity[i];

                if (current.IsActive)
                {
                    var time = current.ActivityAt - before.ActivityAt;
                    if (time <= TimeSpan.FromMilliseconds(_monitoringInterval))
                    {
                        totalActive += time;
                    }
                }
            }

            var lastActivity = activity.LastOrDefault();
            if (lastActivity?.IsActive == true)
            {
                var time = DateTimeOffset.Now - lastActivity.ActivityAt;
                if (time <= TimeSpan.FromMilliseconds(_monitoringInterval))
                {
                    totalActive += time;
                }
            }

            return new TimeOnly(totalActive.Ticks);
        }
    }
}
