using JourneyControl.Application.Models;

namespace JourneyControl.Application.Repositories
{
    public interface IActivityRepository
    {
        List<Activity> GetDateActivity(DateTime reference);
        void Save(Activity model);
    }
}
