namespace JourneyControl.Application.Repositories
{
    public interface ISqlScriptRepository
    {
        Task ExecuteSqlScriptsAsync(List<string> script, IEnumerable<string> connectionStrings, string logicalName, CancellationToken cancellationToken);
    }
}
