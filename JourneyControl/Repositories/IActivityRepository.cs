using JourneyControl.Models;

namespace JourneyControl.Repositories
{
    internal interface IActivityRepository
    {
        List<Activity> GetDateActivity(DateTime reference);
        void Save(Activity model);
    }
}
