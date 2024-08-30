namespace JourneyControl.Application.Services
{
    public interface IActivityService
    {
        void RegisterActivity();
        (TimeOnly time, bool isActive) GetTodayActivity();
    }
}
