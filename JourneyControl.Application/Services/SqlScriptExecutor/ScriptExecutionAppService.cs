using JourneyControl.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace JourneyControl.Application.Services.SqlScriptExecutor
{
    public sealed class ScriptExecutionAppService
    {
        private readonly IDirectoryFinder _directoryFinder;
        private readonly ISqlScriptRepository _sqlRunner;
        private readonly ScriptExecutionOptions _connections;
        private readonly ILogger<ScriptExecutionAppService> _logger;

        private static readonly string[] CorpB2BFolderNames = ["corpb2b", "corp b2b"];
        private const string LojaFolderName = "loja";
        private const string PrecoFolderName = "preco";

        public ScriptExecutionAppService(
            IDirectoryFinder directoryFinder,
            ISqlScriptRepository sqlRunner,
            ScriptExecutionOptions connections,
            ILogger<ScriptExecutionAppService> logger)
        {
            _directoryFinder = directoryFinder;
            _sqlRunner = sqlRunner;
            _connections = connections;
            _logger = logger;
        }

        public async Task ExecuteAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pasta base configurada: {BaseFolder}", _connections.BaseFolder);

            var corpScript = _directoryFinder.FindTargetDirectoryFilesScript(_connections.BaseFolder, CorpB2BFolderNames);
            var lojaScript = _directoryFinder.FindTargetDirectoryFilesScript(_connections.BaseFolder, [LojaFolderName]);
            var precoScript = _directoryFinder.FindTargetDirectoryFilesScript(_connections.BaseFolder, [PrecoFolderName]);


            await _sqlRunner.ExecuteSqlScriptsAsync(corpScript, _connections.CorpB2B, "CorpB2B", cancellationToken);

            await _sqlRunner.ExecuteSqlScriptsAsync(precoScript, _connections.Preco, "Preco", cancellationToken);

            await _sqlRunner.ExecuteSqlScriptsAsync(lojaScript, _connections.LojaConnections, "Loja", cancellationToken);

        }
    }
}
