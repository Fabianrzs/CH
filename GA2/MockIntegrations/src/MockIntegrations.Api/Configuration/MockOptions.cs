namespace MockIntegrations.Api.Configuration;

/// <summary>
/// Configuración raíz del mock (sección <c>MockIntegrations</c> de appsettings o variables
/// de entorno <c>MockIntegrations__*</c>).
/// </summary>
public sealed class MockOptions
{
    public const string Section = "MockIntegrations";

    /// <summary>
    /// Carpeta opcional con fixtures que reemplazan a los embebidos, con la misma estructura
    /// <c>{Proveedor}/Fixtures/{Operación}.{escenario}.{xml|json}</c>. Se leen en cada petición.
    /// </summary>
    public string? FixturesOverridePath { get; set; }

    /// <summary>Retardo (ms) que aplica el escenario integrado <c>slow</c>.</summary>
    public int SlowScenarioDelayMs { get; set; } = 20_000;

    /// <summary>Latencia fija por proveedor (ms), aplicada a todas sus operaciones.</summary>
    public Dictionary<string, int> LatencyMs { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public ScenarioOptions Scenarios { get; set; } = new();

    public JournalOptions Journal { get; set; } = new();

    public SmtpOptions Smtp { get; set; } = new();

    public ServiceBusOptions ServiceBus { get; set; } = new();

    public TusDatosOptions TusDatos { get; set; } = new();

    public KtrionApiOptions KtrionApi { get; set; } = new();

    public TransUnionOptions TransUnion { get; set; } = new();
}

/// <summary>Selección determinista de escenarios.</summary>
public sealed class ScenarioOptions
{
    /// <summary>
    /// Escenario por defecto por proveedor y operación: <c>Defaults:Seven:CrearCDP = "error-negocio"</c>.
    /// La operación <c>*</c> aplica a todas las del proveedor.
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> Defaults { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Reglas por dato de entrada (p. ej. número de documento). Gana la primera que coincide.</summary>
    public List<ScenarioRule> Rules { get; set; } = [];
}

/// <summary>
/// Regla que elige un escenario según un valor del request. El campo se busca por nombre local
/// en el XML SOAP, por nombre de propiedad en el JSON (sin distinguir mayúsculas), en los
/// parámetros de ruta o en la query string.
/// </summary>
public sealed class ScenarioRule
{
    public string Provider { get; set; } = "";

    /// <summary>Operación a la que aplica; vacío o <c>*</c> = todas las del proveedor.</summary>
    public string? Operation { get; set; }

    public string Field { get; set; } = "";

    /// <summary>Valor exacto (sin distinguir mayúsculas). Excluyente con <see cref="StartsWith"/>.</summary>
    public string? EqualsValue { get; set; }

    public string? StartsWith { get; set; }

    public string Scenario { get; set; } = "";

    public string? Description { get; set; }
}

public sealed class JournalOptions
{
    public bool Enabled { get; set; } = true;

    public int Capacity { get; set; } = 500;

    public int MaxBodyChars { get; set; } = 64 * 1024;
}

public sealed class SmtpOptions
{
    public bool Enabled { get; set; } = true;

    /// <summary>Puerto del servidor SMTP de captura (GA2: <c>SMTP:Puerto</c>).</summary>
    public int Port { get; set; } = 2525;

    /// <summary>Destinatarios rechazados con 550 (simula el escenario "destinatario rechazado").</summary>
    public List<string> RejectRecipients { get; set; } = [];

    public int Capacity { get; set; } = 200;
}

public sealed class ServiceBusOptions
{
    /// <summary>
    /// Cadena de conexión del emulador de Azure Service Bus (o de un namespace de pruebas).
    /// Sólo se usa para publicar los eventos que consume GA2. Vacía = publicación deshabilitada.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>Topic fijo en GA2 (<c>EventBusServiceBus.cs</c>).</summary>
    public string Topic { get; set; } = "chonorintegraciones";
}

public sealed class TusDatosOptions
{
    /// <summary>Si tiene valor, el header <c>x-api-key</c> debe coincidir; si no, basta con que exista.</summary>
    public string? ApiKey { get; set; }

    /// <summary>Número de sondeos que devuelven el estado pendiente antes de finalizar el job.</summary>
    public int PollsUntilFinished { get; set; } = 1;

    /// <summary>Valor de <c>statusJobTusDatos</c> mientras el job está en curso (dominio no confirmado).</summary>
    public string PendingStatus { get; set; } = "pendiente";

    /// <summary>Valor de <c>statusJobTusDatos</c> cuando el job termina (dominio no confirmado).</summary>
    public string FinishedStatus { get; set; } = "finalizado";
}

public sealed class KtrionApiOptions
{
    /// <summary>Exige <c>Authorization: Bearer</c> con un token emitido por <c>api/autenticacion</c>.</summary>
    public bool RequireIssuedToken { get; set; } = true;
}

public sealed class TransUnionOptions
{
    /// <summary>PIN OTP que el mock acepta (código 300); cualquier otro devuelve 302.</summary>
    public string ValidPin { get; set; } = "123456";

    /// <summary>Primer ApplicationId que asigna el mock (se incrementa por transacción).</summary>
    public long FirstApplicationId { get; set; } = 9_876_543;
}
