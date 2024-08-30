using JourneyControl.Application.Models;
using JourneyControl.Application.Repositories;

namespace JourneyControl.Infra.DB.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private JourneyControlContext Context { get; }

        public ActivityRepository(JourneyControlContext context)
        {
            Context = context;
        }

        public void Save(Activity model)
        {
            Context.Add(model);
            Context.SaveChanges();
        }

        public List<Activity> GetDateActivity(DateTime reference)
        {
            var startDay = new DateTime(reference.Year, reference.Month, reference.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDay = new DateTime(reference.Year, reference.Month, reference.Day, 23, 59, 59, DateTimeKind.Utc);

            return Context.Activities
                .AsEnumerable()
                .Where(x => x.ActivityAt >= startDay && x.ActivityAt <= endDay)
                .ToList();
        }
    }
}
