using JourneyControl.Models;

namespace JourneyControl.Services
{
    internal interface IActivityService
    {
        void OnChange(Action<Activity> @event);
        void StartMonitoring();
        void StopMonitoring();
        public TimeOnly GetTodayActivity();
    }
}
