using JourneyControl.Models;
using JourneyControl.Repositories;
using System.Timers;

namespace JourneyControl.Services
{
    internal class ActivityService : IActivityService
    {
        private const int _monitoringInterval = 5 * 60 * 1000;

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
                IsActive = lastActivityAt.ToTimeSpan() < TimeSpan.FromMilliseconds( _monitoringInterval)
            };

            ActivityRepository.Save(model);
        }
    }
}
