using JourneyControl.Models;

namespace JourneyControl.Services
{
    internal interface IActivityService
    {
        void StartMonitoring();
        void StopMonitoring();
        public (TimeOnly time, bool isActive) GetTodayActivity();
    }
}
