using System.Collections.Concurrent;
using System.Text;
using MockIntegrations.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Infrastructure.Journal;

/// <summary>Proveedor, operación y escenario de la llamada en curso (los fija cada mock).</summary>
public static class MockCallInfo
{
    private const string Key = "mock.call";

    public static void Set(HttpContext http, string provider, string operation, string scenario, string? source = null)
    {
        http.Items[Key] = new CallInfo(provider, operation, scenario, source);
        http.Response.Headers["X-Mock-Provider"] = provider;
        http.Response.Headers["X-Mock-Operation"] = operation;
        http.Response.Headers["X-Mock-Scenario-Applied"] = scenario;
        if (source is not null)
        {
            http.Response.Headers["X-Mock-Scenario-Source"] = source;
        }
    }

    public static CallInfo? Get(HttpContext http) => http.Items.TryGetValue(Key, out var v) ? v as CallInfo : null;

    public sealed record CallInfo(string Provider, string Operation, string Scenario, string? Source);
}

/// <summary>Una llamada recibida por el mock (para aserciones en pruebas de integración).</summary>
public sealed record JournalEntry
{
    public required long Id { get; init; }

    public required DateTimeOffset Timestamp { get; init; }

    public required string Method { get; init; }

    public required string Path { get; init; }

    public string? Query { get; init; }

    public string? Provider { get; init; }

    public string? Operation { get; init; }

    public string? Scenario { get; init; }

    public string? ScenarioSource { get; init; }

    public required int StatusCode { get; init; }

    public required double DurationMs { get; init; }

    /// <summary>Headers recibidos; <c>Authorization</c> y <c>x-api-key</c> se enmascaran.</summary>
    public required IReadOnlyDictionary<string, string> Headers { get; init; }

    public string? RequestBody { get; init; }

    public bool RequestBodyTruncated { get; init; }
}

/// <summary>Registro circular en memoria de las llamadas recibidas.</summary>
public sealed class RequestJournal(IOptionsMonitor<MockOptions> options)
{
    private readonly ConcurrentQueue<JournalEntry> _entries = new();
    private long _sequence;

    public long NextId() => Interlocked.Increment(ref _sequence);

    public void Add(JournalEntry entry)
    {
        _entries.Enqueue(entry);
        while (_entries.Count > Math.Max(1, options.CurrentValue.Journal.Capacity) && _entries.TryDequeue(out _))
        {
        }
    }

    public IReadOnlyList<JournalEntry> Query(string? provider, string? operation, int take) =>
        _entries.Reverse()
            .Where(e => provider is null || string.Equals(e.Provider, provider, StringComparison.OrdinalIgnoreCase))
            .Where(e => operation is null || string.Equals(e.Operation, operation, StringComparison.OrdinalIgnoreCase))
            .Take(Math.Clamp(take, 1, 1000))
            .ToList();

    public void Clear() => _entries.Clear();
}

/// <summary>Guarda cada llamada a los mocks (no registra Swagger, OpenAPI ni la administración).</summary>
public sealed class RequestJournalMiddleware(RequestDelegate next, RequestJournal journal, IOptionsMonitor<MockOptions> options)
{
    private static readonly string[] Ignored = ["/swagger", "/openapi", "/_mock", "/favicon.ico", "/health"];
    private static readonly string[] Masked = ["Authorization", "x-api-key"];

    public async Task InvokeAsync(HttpContext http)
    {
        var path = http.Request.Path.Value ?? "/";
        if (!options.CurrentValue.Journal.Enabled || path == "/" || Ignored.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await next(http);
            return;
        }

        http.Request.EnableBuffering();
        var max = options.CurrentValue.Journal.MaxBodyChars;
        string? body = null;
        var truncated = false;
        if (http.Request.ContentLength is not 0)
        {
            using var reader = new StreamReader(http.Request.Body, Encoding.UTF8, leaveOpen: true);
            body = await reader.ReadToEndAsync(http.RequestAborted);
            http.Request.Body.Position = 0;
            if (body.Length > max)
            {
                body = body[..max];
                truncated = true;
            }
        }

        var started = DateTimeOffset.UtcNow;
        try
        {
            await next(http);
        }
        finally
        {
            var call = MockCallInfo.Get(http);
            journal.Add(new JournalEntry
            {
                Id = journal.NextId(),
                Timestamp = started,
                Method = http.Request.Method,
                Path = path,
                Query = http.Request.QueryString.HasValue ? http.Request.QueryString.Value : null,
                Provider = call?.Provider,
                Operation = call?.Operation,
                Scenario = call?.Scenario,
                ScenarioSource = call?.Source,
                StatusCode = http.Response.StatusCode,
                DurationMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds,
                Headers = http.Request.Headers.ToDictionary(
                    h => h.Key,
                    h => Masked.Contains(h.Key, StringComparer.OrdinalIgnoreCase) ? Mask(h.Value.ToString()) : h.Value.ToString(),
                    StringComparer.OrdinalIgnoreCase),
                RequestBody = body,
                RequestBodyTruncated = truncated,
            });
        }
    }

    private static string Mask(string value) =>
        value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? "Bearer ***" : (value.Length == 0 ? "" : "***");
}
