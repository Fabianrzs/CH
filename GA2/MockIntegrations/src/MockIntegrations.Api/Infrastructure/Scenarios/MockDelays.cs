using MockIntegrations.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Infrastructure.Scenarios;

/// <summary>Latencia configurada por proveedor y escenario <c>slow</c>.</summary>
public sealed class MockDelays(IOptionsMonitor<MockOptions> options)
{
    public Task ApplyAsync(string provider, string scenario, CancellationToken cancellationToken)
    {
        var current = options.CurrentValue;
        var latency = current.LatencyMs.FirstOrDefault(kv => string.Equals(kv.Key, provider, StringComparison.OrdinalIgnoreCase)).Value;
        if (scenario == BuiltInScenarios.Slow)
        {
            latency += current.SlowScenarioDelayMs;
        }

        return latency > 0 ? Task.Delay(latency, cancellationToken) : Task.CompletedTask;
    }
}
