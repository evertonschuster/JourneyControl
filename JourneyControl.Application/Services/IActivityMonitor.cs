namespace JourneyControl.Application.Services
{
    public interface IActivityMonitor
    {
        TimeOnly GetLastActivity();
    }
}
