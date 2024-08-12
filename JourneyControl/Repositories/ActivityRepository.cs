using JourneyControl.Models;

namespace JourneyControl.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private JourneyControlContext Context { get; }

        public ActivityRepository(JourneyControlContext context)
        {
            this.Context = context;
        }

        public void Save(Activity model)
        {
            Context.Add(model);
            Context.SaveChanges();
        }
    }
}
