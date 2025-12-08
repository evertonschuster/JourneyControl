namespace JourneyControl.Application.Services.SqlScriptExecutor
{
    public interface IDirectoryFinder
    {
        List<string> FindTargetDirectoryFilesScript(string rootPath, string[] targetNames);
    }
}
