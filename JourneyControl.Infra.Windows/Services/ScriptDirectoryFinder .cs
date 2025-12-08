using JourneyControl.Application.Services.SqlScriptExecutor;

namespace JourneyControl.Infra.Windows.Services
{
    public sealed class ScriptDirectoryFinder : IDirectoryFinder
    {
        public List<string> FindTargetDirectoryFilesScript(string rootPath, string[] targetNames)
        {
            if (rootPath is null)
                throw new ArgumentNullException(nameof(rootPath));

            if (targetNames is null)
                throw new ArgumentNullException(nameof(targetNames));

            var normalizedTargets = CreateNormalizedTargetSet(targetNames);
            var secondLevelDirs = GetSecondLevelDirectories(rootPath);

            return secondLevelDirs
                .Where(dir => DirectoryIsTarget(dir, normalizedTargets))
                .SelectMany(ReadSqlScriptsFromDirectory)
                .ToList();
        }

        private static HashSet<string> CreateNormalizedTargetSet(IEnumerable<string> targetNames)
        {
            return new HashSet<string>(
                targetNames.Select(NormalizeName),
                StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> GetSecondLevelDirectories(string rootPath)
        {
            return Directory.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly)
                .SelectMany(firstLevelDir =>
                    Directory.EnumerateDirectories(firstLevelDir, "*", SearchOption.TopDirectoryOnly));
        }

        private static bool DirectoryIsTarget(string directoryPath, HashSet<string> normalizedTargets)
        {
            var dirName = Path.GetFileName(directoryPath) ?? string.Empty;
            return normalizedTargets.Contains(NormalizeName(dirName));
        }

        private static IEnumerable<string> ReadSqlScriptsFromDirectory(string directoryPath)
        {
            return Directory
                .EnumerateFiles(directoryPath, "*.sql", SearchOption.TopDirectoryOnly)
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .Select(File.ReadAllText);
        }

        private static string NormalizeName(string name)
        {
            return name
                .Trim()
                .Replace(" ", string.Empty, StringComparison.Ordinal)
                .ToLowerInvariant();
        }
    }
}
