using JourneyControl.Application.Services.SqlScriptExecutor;
using Microsoft.Data.SqlClient;

namespace App.WindowsService
{
    internal class ScriptExecutionWorker : BackgroundService
    {
        private readonly ScriptExecutionAppService _appService;
        private readonly ILogger<ScriptExecutionWorker> _logger;

        public ScriptExecutionWorker(
            ScriptExecutionAppService appService,
            ILogger<ScriptExecutionWorker> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado em {Time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _appService.ExecuteAllAsync(stoppingToken);

                    // Se chegou aqui, executou com sucesso -> sai do loop
                    break;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Execução cancelada.");
                    break;
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "Erro de SQL ao executar scripts. Nova tentativa em 30 minutos.");

                    try
                    {
                        // Espera 30 minutos antes de tentar de novo
                        await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogWarning("Execução cancelada durante o tempo de espera.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado ao executar scripts. Worker será finalizado.");
                    break;
                }
            }

            _logger.LogInformation("Worker finalizado em {Time}", DateTimeOffset.Now);
        }
    }
}
