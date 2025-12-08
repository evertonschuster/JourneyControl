using JourneyControl.Application.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace JourneyControl.Infra.DB.Repositories
{
    public class SqlScriptRepository : ISqlScriptRepository
    {
        private readonly ILogger<SqlScriptRepository> _logger;

        private static readonly Regex BatchSplitter = new(
            @"^\s*GO\s*;$|^\s*GO\s*$",
            RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public SqlScriptRepository(ILogger<SqlScriptRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteSqlScriptsAsync(
            List<string> sqlFiles,
            IEnumerable<string> connectionStrings,
            string logicalName,
            CancellationToken cancellationToken)
        {
            if (sqlFiles is null)
                throw new ArgumentNullException(nameof(sqlFiles));

            if (connectionStrings is null)
                throw new ArgumentNullException(nameof(connectionStrings));

            if (sqlFiles.Count == 0)
            {
                _logger.LogInformation("[{LogicalName}] Nenhum arquivo .sql encontrado.", logicalName);
                return;
            }

            foreach (var connectionString in connectionStrings)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation(
                    "[{LogicalName}] Conectando em -> {ConnectionPreview}",
                    logicalName,
                    GetConnectionPreview(connectionString));

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);

                foreach (var script in sqlFiles)
                {
                    await ExecuteScriptAsync(connection, script, logicalName, cancellationToken);
                }
            }
        }

        private async Task ExecuteScriptAsync(
            SqlConnection connection,
            string script,
            string logicalName,
            CancellationToken cancellationToken)
        {
            foreach (var batch in SplitSqlBatches(script))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(batch))
                    continue;

                await using var command = new SqlCommand(batch, connection)
                {
                    CommandTimeout = 0
                };

                try
                {
                    await command.ExecuteNonQueryAsync(cancellationToken);
                    _logger.LogInformation("[{LogicalName}] Batch executado com sucesso.", logicalName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "[{LogicalName}] Erro ao executar o batch: {ErrorMessage}, {Batch}",
                        logicalName,
                        ex.Message,
                        batch);
                }
            }
        }

        private static IEnumerable<string> SplitSqlBatches(string script)
        {
            return BatchSplitter
                .Split(script)
                .Where(batch => !string.IsNullOrWhiteSpace(batch));
        }

        private static string GetConnectionPreview(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return string.Empty;

            const int maxLength = 120;
            return connectionString.Length <= maxLength
                ? connectionString
                : connectionString[..maxLength];
        }
    }
}
