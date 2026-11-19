using System.Collections.Concurrent;
using System.Text;
using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Rest;
using MockIntegrations.Api.Providers.TusDatos.Models;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.TusDatos;

/// <summary>
/// E8 — TusDatos detrás del API Gateway de Caja Honor (<c>kiosko-dev</c>). REST/JSON con <c>x-api-key</c>.
/// Patrón asíncrono: la consulta devuelve un <c>jobId</c> y GA2 sondea el job. Contrato: docs/providers/E08-tusdatos.md.
/// GA2 trata el 404 como éxito con <c>Data = null</c> y cualquier otro error como <c>IsSuccess = false</c> con el cuerpo crudo.
/// </summary>
public static class TusDatosMock
{
    public const string Provider = "TusDatos";
    private const string Base = "/kiosko-dev/api/v1";
    private const string ConfigKey = "Integrationes:TusDatos:BaseUrl (+ Version, Controller, Endpoints:*)";
    private const string Source = "ReferenciasSvc/TusDatos/TusDatosClient.cs (CommonIntegracionCliente)";

    private static readonly ScenarioInfo NotFound = new("no-encontrado", "HTTP 404: GA2 lo trata como éxito con Data=null ('Resource not found').");
    private static readonly ScenarioInfo Unauthorized = new("unauthorized", "HTTP 401 con cuerpo de texto (también si falta x-api-key): GA2 devuelve IsSuccess=false con ese texto.");

    public static readonly RestOperationSpec Consultar = new()
    {
        Provider = Provider,
        Name = "ConsultarCliente",
        Usage = Ga2Usage.Business,
        Summary = "Consulta del cliente en TusDatos (inicia un job asíncrono)",
        Description = "GA2 la invoca desde `GET api/terceros/validacion/riesgos/tusdatos` y `GET api/integrations/tusdatos/customer/...`. "
            + "`checkHistory` llega en minúsculas (`true`/`false`). El `jobId` devuelto se sondea con `ConsultarJob`.",
        Scenarios =
        [
            new("pep", "Inicia un job que al finalizar marca isPEPS=true y detectionRisk='ALTO'."),
            new("historial", "Resultado del histórico: jobId=null, isFromHistory=true, historyDataCustomer {isPEPS=false, detectionRisk='BAJO'}."),
            NotFound, Unauthorized,
        ],
        Ga2ConfigKey = ConfigKey,
        Ga2Source = Source,
    };

    public static readonly RestOperationSpec ConsultarJob = new()
    {
        Provider = Provider,
        Name = "ConsultarJob",
        Usage = Ga2Usage.Business,
        Summary = "Sondeo del job de TusDatos",
        Description = "Devuelve `MockIntegrations:TusDatos:PendingStatus` durante los primeros `PollsUntilFinished` sondeos y luego "
            + "`FinishedStatus` con `historyDataCustomer`. Un jobId desconocido responde 404. **El dominio real de `statusJobTusDatos` no está confirmado.**",
        Scenarios = [NotFound, Unauthorized],
        Ga2ConfigKey = ConfigKey,
        Ga2Source = Source,
        Certainty = ContractCertainty.StructureOnly,
    };

    public static readonly RestOperationSpec ConsultarVigia = new()
    {
        Provider = Provider,
        Name = "ConsultarVigia",
        Usage = Ga2Usage.Business,
        Summary = "Riesgo LA/FT (Vigía a través del gateway)",
        Description = "Ruta sin el segmento `tusdatos` (GA2 reemplaza `api/v1/tusdatos` por `api/v1`). GA2 devuelve la respuesta al frontend sin interpretar `riskLAFT`; **su dominio no está confirmado**.",
        Scenarios = [new("riesgo", "riskLAFT='RIESGO ALTO'."), NotFound, Unauthorized],
        Ga2ConfigKey = ConfigKey,
        Ga2Source = Source,
        Certainty = ContractCertainty.StructureOnly,
    };

    public static void Map(WebApplication app)
    {
        app.MapMock("GET", Base + "/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{checkHistory}", Consultar, ConsultarAsync)
            .Produces<DatosClienteResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain")
            .Produces<string>(StatusCodes.Status500InternalServerError, "text/plain");

        app.MapMock("GET", Base + "/tusdatos/jobid/{jobId}/customer/{identification}", ConsultarJob, ConsultarJobAsync)
            .Produces<DatosClienteJobResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain")
            .Produces<string>(StatusCodes.Status500InternalServerError, "text/plain");

        app.MapMock("GET", Base + "/vigia/customer/{identification}/document-type/{documentType}/CheckRiskLAFT", ConsultarVigia, ConsultarVigiaAsync)
            .Produces<VigiaResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain")
            .Produces<string>(StatusCodes.Status500InternalServerError, "text/plain");
    }

    private static async Task<IResult> ConsultarAsync(HttpContext http, string identification, string documentType, string checkHistory,
        TusDatosState state, IOptionsMonitor<MockOptions> options)
    {
        var decision = await RestMock.BeginAsync(http, Consultar, RestMock.Lookup(http));
        if ((Guard(http, decision.Name, options.CurrentValue.TusDatos) ?? RestMock.BuiltIn(decision, Consultar)) is { } early)
        {
            return early;
        }

        return decision.Name switch
        {
            "default" or "pep" => Results.Ok(new DatosClienteResponse
            {
                JobId = state.Start(identification, pep: decision.Name == "pep"),
                IsFromHistory = false,
                HistoryDataCustomer = null,
            }),
            "historial" => Results.Ok(new DatosClienteResponse
            {
                JobId = null,
                IsFromHistory = true,
                HistoryDataCustomer = new HistoryDataCustomer { IsPEPS = false, DetectionRisk = "BAJO" },
            }),
            "no-encontrado" => Results.NotFound(),
            _ => RestMock.UnknownScenario(decision, Consultar),
        };
    }

    private static async Task<IResult> ConsultarJobAsync(HttpContext http, string jobId, string identification,
        TusDatosState state, IOptionsMonitor<MockOptions> options)
    {
        var settings = options.CurrentValue.TusDatos;
        var decision = await RestMock.BeginAsync(http, ConsultarJob, RestMock.Lookup(http));
        if ((Guard(http, decision.Name, settings) ?? RestMock.BuiltIn(decision, ConsultarJob)) is { } early)
        {
            return early;
        }

        if (decision.Name == "no-encontrado")
        {
            return Results.NotFound();
        }

        if (decision.Name != "default")
        {
            return RestMock.UnknownScenario(decision, ConsultarJob);
        }

        if (state.Poll(jobId) is not { } job)
        {
            return Results.NotFound();
        }

        var finished = job.Polls > settings.PollsUntilFinished;
        return Results.Ok(new DatosClienteJobResponse
        {
            StatusJobTusDatos = finished ? settings.FinishedStatus : settings.PendingStatus,
            HistoryDataCustomer = finished
                ? new HistoryDataCustomer { IsPEPS = job.Pep, DetectionRisk = job.Pep ? "ALTO" : "BAJO" }
                : null,
        });
    }

    private static async Task<IResult> ConsultarVigiaAsync(HttpContext http, string identification, string documentType, IOptionsMonitor<MockOptions> options)
    {
        var decision = await RestMock.BeginAsync(http, ConsultarVigia, RestMock.Lookup(http));
        if ((Guard(http, decision.Name, options.CurrentValue.TusDatos) ?? RestMock.BuiltIn(decision, ConsultarVigia)) is { } early)
        {
            return early;
        }

        return decision.Name switch
        {
            "default" or "riesgo" => Results.Ok(new VigiaResponse
            {
                Identification = identification,
                FirstName = "JUAN CARLOS",
                LastName = "PEREZ GOMEZ",
                RiskLAFT = decision.Name == "riesgo" ? "RIESGO ALTO" : "SIN RIESGO",
                DocumentType = documentType,
            }),
            "no-encontrado" => Results.NotFound(),
            _ => RestMock.UnknownScenario(decision, ConsultarVigia),
        };
    }

    /// <summary>El gateway exige <c>x-api-key</c>; si se configuró un valor esperado, debe coincidir.</summary>
    private static IResult? Guard(HttpContext http, string scenario, TusDatosOptions settings)
    {
        var key = http.Request.Headers["x-api-key"].ToString();
        var invalid = string.IsNullOrEmpty(key) || (!string.IsNullOrEmpty(settings.ApiKey) && key != settings.ApiKey);
        return invalid || scenario == "unauthorized"
            ? Results.Text("Unauthorized: x-api-key inválida o ausente (MockIntegrations)", "text/plain", Encoding.UTF8, StatusCodes.Status401Unauthorized)
            : null;
    }
}

/// <summary>Jobs de TusDatos en curso (en memoria).</summary>
public sealed class TusDatosState
{
    private readonly ConcurrentDictionary<string, Job> _jobs = new(StringComparer.OrdinalIgnoreCase);
    private long _next;

    public string Start(string identification, bool pep)
    {
        var id = $"mock-job-{Interlocked.Increment(ref _next):D6}";
        _jobs[id] = new Job { Identification = identification, Pep = pep };
        return id;
    }

    /// <summary>Cuenta un sondeo y devuelve el estado actualizado, o <c>null</c> si el job no existe.</summary>
    public JobSnapshot? Poll(string jobId)
    {
        if (!_jobs.TryGetValue(jobId, out var job))
        {
            return null;
        }

        return new JobSnapshot(job.Identification, job.Pep, Interlocked.Increment(ref job.Polls));
    }

    public void Reset()
    {
        _jobs.Clear();
        Interlocked.Exchange(ref _next, 0);
    }

    private sealed class Job
    {
        public required string Identification { get; init; }

        public required bool Pep { get; init; }

        public int Polls;
    }

    public sealed record JobSnapshot(string Identification, bool Pep, int Polls);
}
