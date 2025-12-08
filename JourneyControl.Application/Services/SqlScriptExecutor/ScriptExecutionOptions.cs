using Microsoft.Extensions.Configuration;

namespace JourneyControl.Application.Services.SqlScriptExecutor
{
    public class ScriptExecutionOptions
    {
        public string BaseFolder { get; set; }
        public List<string> CorpB2B { get; set; }
        public List<string> Preco { get; set; }
        public List<string> LojaConnections { get; set; }


        public ScriptExecutionOptions(IConfiguration configuration)
        {
            BaseFolder = configuration["BaseFolder"] ?? throw new InvalidOperationException("BaseFolder não configurado em appsettings.json.");

            CorpB2B = GetRequiredConnectionString(configuration, "CorpB2B");
            Preco = GetRequiredConnectionString(configuration, "Preco");

            var cb = GetRequiredConnectionString(configuration, "CB");
            var pf = GetRequiredConnectionString(configuration, "PF");
            var ex = GetRequiredConnectionString(configuration, "EX");


            LojaConnections = [.. cb, .. pf, .. ex];
        }

        private static List<string> GetRequiredConnectionString(IConfiguration configuration, string name)
        {
            var section = configuration.GetSection($"ConnectionStrings:{name}");
            var connections = section.GetChildren();

            if (connections.Any())
            {
                return connections.Select(c => c.Value!).Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
            }

            throw new InvalidOperationException(
                $"ConnectionStrings '{name}' não encontrada ou vazia em ConnectionStrings.");
        }
    }
}