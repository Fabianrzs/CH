using System.Collections.Concurrent;
using MockIntegrations.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Infrastructure.Scenarios;

/// <summary>Escenarios integrados, disponibles en todas las operaciones.</summary>
public static class BuiltInScenarios
{
    /// <summary>Respuesta de éxito (fixture <c>{Operación}.default</c>).</summary>
    public const string Default = "default";

    /// <summary>SOAP: HTTP 500 con <c>s:Fault</c>. REST: HTTP 500 con cuerpo de texto.</summary>
    public const string Fault = "fault";

    /// <summary>HTTP 500 con cuerpo HTML (no SOAP): provoca <c>ProtocolException</c>/<c>CommunicationException</c> en WCF.</summary>
    public const string Http500 = "http-500";

    /// <summary>Respuesta normal tras <c>MockIntegrations:SlowScenarioDelayMs</c>.</summary>
    public const string Slow = "slow";

    public static readonly IReadOnlyList<(string Name, string Description)> All =
    [
        (Default, "Respuesta de éxito."),
        (Fault, "Error técnico: SOAP Fault HTTP 500 (SOAP) o HTTP 500 con texto (REST)."),
        (Http500, "HTTP 500 con HTML, sin sobre SOAP (falla de infraestructura)."),
        (Slow, "Respuesta de éxito con retardo (MockIntegrations:SlowScenarioDelayMs)."),
    ];
}

/// <summary>Origen de la decisión de escenario (se devuelve en el header <c>X-Mock-Scenario-Source</c>).</summary>
public enum ScenarioSource
{
    Header,
    RuntimeOverride,
    Rule,
    ConfiguredDefault,
    Default,
}

public sealed record ScenarioDecision(string Name, ScenarioSource Source, string? Detail = null);

/// <summary>Escenarios fijados en caliente con la API de administración (<c>/_mock/scenarios</c>).</summary>
public sealed class ScenarioOverrideStore
{
    private readonly ConcurrentDictionary<string, string> _overrides = new(StringComparer.OrdinalIgnoreCase);

    public static string Key(string provider, string operation) => $"{provider}/{operation}";

    public void Set(string provider, string operation, string scenario) => _overrides[Key(provider, operation)] = scenario;

    public bool Remove(string provider, string operation) => _overrides.TryRemove(Key(provider, operation), out _);

    public void Clear() => _overrides.Clear();

    public string? Get(string provider, string operation) =>
        _overrides.TryGetValue(Key(provider, operation), out var s) ? s
        : _overrides.TryGetValue(Key(provider, "*"), out var all) ? all
        : null;

    public IReadOnlyDictionary<string, string> Snapshot() => new Dictionary<string, string>(_overrides);
}

/// <summary>
/// Decide el escenario de una llamada de forma determinista. Prioridad:
/// 1) header <c>X-Mock-Scenario</c> (pruebas manuales; GA2 nunca lo envía),
/// 2) override en caliente (<c>PUT /_mock/scenarios/...</c>),
/// 3) reglas por dato de entrada (<c>MockIntegrations:Scenarios:Rules</c>),
/// 4) escenario por defecto configurado (<c>MockIntegrations:Scenarios:Defaults</c>),
/// 5) <c>default</c>.
/// </summary>
public sealed class ScenarioResolver(IOptionsMonitor<MockOptions> options, ScenarioOverrideStore overrides)
{
    public const string ScenarioHeader = "X-Mock-Scenario";

    public ScenarioDecision Resolve(HttpContext http, string provider, string operation, Func<string, string?> lookup)
    {
        if (http.Request.Headers.TryGetValue(ScenarioHeader, out var header) && !string.IsNullOrWhiteSpace(header))
        {
            return new(header.ToString().Trim(), ScenarioSource.Header);
        }

        if (overrides.Get(provider, operation) is { } overridden)
        {
            return new(overridden, ScenarioSource.RuntimeOverride);
        }

        var scenarios = options.CurrentValue.Scenarios;
        foreach (var rule in scenarios.Rules)
        {
            if (!string.Equals(rule.Provider, provider, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!string.IsNullOrEmpty(rule.Operation) && rule.Operation != "*"
                && !string.Equals(rule.Operation, operation, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var value = lookup(rule.Field);
            if (value is null)
            {
                continue;
            }

            var matches = rule.EqualsValue is not null
                ? string.Equals(value.Trim(), rule.EqualsValue, StringComparison.OrdinalIgnoreCase)
                : rule.StartsWith is not null && value.Trim().StartsWith(rule.StartsWith, StringComparison.OrdinalIgnoreCase);

            if (matches)
            {
                return new(rule.Scenario, ScenarioSource.Rule, $"{rule.Field}={value}");
            }
        }

        // El binder de configuración crea diccionarios que distinguen mayúsculas: se compara a mano.
        var perOperation = FindIgnoreCase(scenarios.Defaults, provider);
        if (perOperation is not null
            && (FindIgnoreCase(perOperation, operation) ?? FindIgnoreCase(perOperation, "*")) is { } configured)
        {
            return new(configured, ScenarioSource.ConfiguredDefault);
        }

        return new(BuiltInScenarios.Default, ScenarioSource.Default);
    }

    internal static T? FindIgnoreCase<T>(IReadOnlyDictionary<string, T> source, string key) where T : class =>
        source.FirstOrDefault(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)).Value;
}
