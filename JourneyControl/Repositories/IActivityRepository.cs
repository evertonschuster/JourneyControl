using JourneyControl.Models;

namespace JourneyControl.Repositories
{
    internal interface IActivityRepository
    {
        void Save(Activity model);
    }
}
