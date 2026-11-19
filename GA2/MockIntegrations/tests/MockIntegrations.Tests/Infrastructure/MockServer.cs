using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MockIntegrations.Tests.Infrastructure;

/// <summary>
/// Levanta MockIntegrations en Kestrel (HTTP real, puerto libre) para que los proxies WCF y los
/// clientes HTTP de GA2 se comuniquen con él igual que en un entorno de pruebas.
/// </summary>
public sealed class MockServer : IAsyncLifetime
{
    private WebApplicationFactory<Program>? _factory;

    public Uri BaseUri { get; private set; } = null!;

    public int SmtpPort { get; private set; }

    public HttpClient Http { get; private set; } = null!;

    public IServiceProvider Services => _factory!.Services;

    public ValueTask InitializeAsync()
    {
        var httpPort = FreePort();
        SmtpPort = FreePort();
        BaseUri = new Uri($"http://127.0.0.1:{httpPort}/");

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.UseSetting("Urls", BaseUri.ToString());
            builder.UseSetting("MockIntegrations:Smtp:Port", SmtpPort.ToString());
            builder.UseSetting("MockIntegrations:SlowScenarioDelayMs", "300");
        });
        _factory.UseKestrel(httpPort);
        _factory.StartServer();

        Http = new HttpClient { BaseAddress = BaseUri };
        return ValueTask.CompletedTask;
    }

    public T GetService<T>() where T : notnull => Services.GetRequiredService<T>();

    /// <summary>Reinicia overrides, contadores, sesiones, registro y buzón.</summary>
    public async Task ResetAsync()
    {
        using var response = await Http.PostAsync("_mock/reset", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task SetScenarioAsync(string provider, string operation, string scenario)
    {
        using var response = await Http.PutAsJsonAsync($"_mock/scenarios/{provider}/{operation}", new { scenario });
        response.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        Http?.Dispose();
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }
    }

    private static int FreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }
}

/// <summary>Todas las pruebas comparten un servidor y se ejecutan en serie (el estado del mock es global).</summary>
[CollectionDefinition(Name)]
public sealed class MockServerCollection : ICollectionFixture<MockServer>
{
    public const string Name = "MockServer";
}
