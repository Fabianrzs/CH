using System.Collections.Concurrent;

namespace MockIntegrations.Api.Infrastructure.Catalog;

public enum MockProtocol
{
    Soap11,
    Rest,
    Smtp,
    ServiceBus,
}

/// <summary>Cómo usa GA2 la operación (según el análisis del código, ver docs/).</summary>
public enum Ga2Usage
{
    /// <summary>La invoca la lógica de negocio de GA2.</summary>
    Business,

    /// <summary>Sólo la expone un endpoint de GA2 que reenvía la llamada tal cual.</summary>
    PassThrough,

    /// <summary>Está en el contrato/cliente pero ningún flujo de GA2 la invoca.</summary>
    NotUsed,
}

/// <summary>Una operación simulada. Es la fuente única para el catálogo, OpenAPI y las pruebas.</summary>
public sealed record MockOperation
{
    public required string Provider { get; init; }

    public required string Name { get; init; }

    public required MockProtocol Protocol { get; init; }

    /// <summary>Verbo HTTP (REST/SOAP) o canal (SMTP/Service Bus).</summary>
    public required string Method { get; init; }

    /// <summary>Ruta servida por el mock.</summary>
    public required string Route { get; init; }

    /// <summary>SOAPAction (sólo SOAP).</summary>
    public string? SoapAction { get; init; }

    public required Ga2Usage Usage { get; init; }

    public required string Summary { get; init; }

    /// <summary>Escenarios soportados (además de los integrados).</summary>
    public IReadOnlyList<ScenarioInfo> Scenarios { get; init; } = [];

    /// <summary>Clave de configuración de GA2 que apunta a esta operación.</summary>
    public string? Ga2ConfigKey { get; init; }

    /// <summary>Código de GA2 que construye el request o lee el response.</summary>
    public string? Ga2Source { get; init; }

    /// <summary>Grado de certeza del contrato.</summary>
    public ContractCertainty Certainty { get; init; } = ContractCertainty.Validated;

    public string? Notes { get; init; }
}

public sealed record ScenarioInfo(string Name, string Description);

public enum ContractCertainty
{
    /// <summary>Estructura validada por ida y vuelta con el proxy/DTO real de GA2.</summary>
    Validated,

    /// <summary>Estructura validada, pero el contenido de negocio (códigos, dominios) no está confirmado con el proveedor.</summary>
    StructureOnly,

    /// <summary>GA2 no la usa; el mock sólo responde un error explícito.</summary>
    NotImplemented,
}

/// <summary>Registro en memoria de todas las operaciones simuladas.</summary>
public sealed class MockCatalog
{
    private readonly ConcurrentDictionary<(string Provider, string Operation), MockOperation> _operations =
        new(new ProviderOperationComparer());

    public IReadOnlyCollection<MockOperation> Operations =>
        _operations.Values.OrderBy(o => o.Provider).ThenBy(o => o.Route).ThenBy(o => o.Name).ToList();

    public void Add(MockOperation operation)
    {
        if (!_operations.TryAdd((operation.Provider, operation.Name), operation))
        {
            throw new InvalidOperationException($"Operación duplicada: {operation.Provider}/{operation.Name}");
        }
    }

    public MockOperation? Find(string provider, string operation) =>
        _operations.TryGetValue((provider, operation), out var op) ? op : null;

    public IEnumerable<string> Providers => _operations.Keys.Select(k => k.Provider).Distinct(StringComparer.OrdinalIgnoreCase);

    private sealed class ProviderOperationComparer : IEqualityComparer<(string Provider, string Operation)>
    {
        public bool Equals((string Provider, string Operation) x, (string Provider, string Operation) y) =>
            string.Equals(x.Provider, y.Provider, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.Operation, y.Operation, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode((string Provider, string Operation) obj) =>
            HashCode.Combine(obj.Provider.ToUpperInvariant(), obj.Operation.ToUpperInvariant());
    }
}
