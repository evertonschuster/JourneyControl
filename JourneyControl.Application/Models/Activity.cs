namespace JourneyControl.Application.Models
{
    public class Activity
    {
        public int Id { get; init; }

        public DateTimeOffset ActivityAt { get; init; }

        public TimeOnly Inactivity { get; init; }

        public bool IsActive { get; init; }

    }
}
