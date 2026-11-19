using MockIntegrations.Api.Infrastructure.Admin;
using MockIntegrations.Api.Providers.Bua;
using MockIntegrations.Api.Providers.Dodo;
using MockIntegrations.Api.Providers.Fenix;
using MockIntegrations.Api.Providers.Haberes;
using MockIntegrations.Api.Providers.KtrionApi;
using MockIntegrations.Api.Providers.Seven;
using MockIntegrations.Api.Providers.ServiceBus;
using MockIntegrations.Api.Providers.Smtp;
using MockIntegrations.Api.Providers.TransUnion;
using MockIntegrations.Api.Providers.TusDatos;
using MockIntegrations.Api.Providers.Vigia;
using MockIntegrations.Api.Providers.WorkManager;

namespace MockIntegrations.Api.Providers;

/// <summary>Un proveedor simulado: su documento OpenAPI (<c>/openapi/{Id}.json</c>) y su registro de endpoints.</summary>
public sealed record ProviderInfo(string Key, string Title, string Protocol, string Description, Action<WebApplication> Map)
{
    /// <summary>Nombre del documento OpenAPI y del grupo de endpoints.</summary>
    public string Id => Key.ToLowerInvariant();
}

public static class ProviderRegistry
{
    public static readonly IReadOnlyList<ProviderInfo> All =
    [
        new(SevenMock.Provider, "E1 · Seven ERP", "SOAP 1.1 (7 servicios ASMX)",
            "ERP contable de Digital Ware: comprobantes, CDP, facturas, terceros y proveedores.", SevenMock.Map),
        new(WorkManagerMock.Provider, "E2 · WorkManager / DODO (SOAP)", "SOAP 1.1 (WCF)",
            "Gestor documental y BPM: radicación, adjuntos y formularios.", WorkManagerMock.Map),
        new(BuaMock.Provider, "E3 · BUA SOAP", "SOAP 1.1 (WCF)",
            "Base Única de Afiliados de Caja Honor (ws.01 y ws.02).", BuaMock.Map),
        new(VigiaMock.Provider, "E4 · Vigía", "SOAP 1.1 RPC/encoded",
            "Verificación de terceros en listas LA/FT (SARLAFT).", VigiaMock.Map),
        new(FenixMock.Provider, "E5 · Fénix (biometría)", "SOAP 1.1 (ASMX, DataSet)",
            "Validación biométrica del afiliado.", FenixMock.Map),
        new(HaberesMock.Provider, "E6 · Haberes (MCD)", "SOAP 1.1 (ASMX)",
            "Certificados y reportes de haberes.", HaberesMock.Map),
        new(TransUnionMock.Provider, "E7 · TransUnion DecisionCentre", "SOAP 1.1 + XML en texto",
            "IDVision/OTP, score AQM e historial crediticio.", TransUnionMock.Map),
        new(TusDatosMock.Provider, "E8 · TusDatos (API Gateway)", "REST/JSON",
            "Validación PEP/riesgo y Vigía LA/FT vía API Gateway de Caja Honor.", TusDatosMock.Map),
        new(DodoMock.Provider, "E9 · Dodo REST", "REST/JSON",
            "Nueva API REST del gestor documental (pass-through en GA2).", DodoMock.Map),
        new(KtrionApiMock.Provider, "E10 · Pasarela ktrionApi", "REST/JSON + Bearer",
            "Pasarela de Caja Honor hacia WorkManager y Haberes.", KtrionApiMock.Map),
        new(SmtpMock.Provider, "E11 · SMTP", "SMTP (TCP) + HTTP de inspección",
            "Servidor SMTP de captura y consulta de los correos recibidos.", SmtpMock.Map),
        new(ServiceBusMock.Provider, "E12 · Azure Service Bus", "AMQP (emulador) + HTTP de publicación",
            "Publicación de los eventos que consume GA2 en el emulador de Service Bus.", ServiceBusMock.Map),
        new(AdminEndpoints.Provider, "Administración del mock", "REST/JSON",
            "Catálogo, escenarios, registro de llamadas y reinicio de estado.", AdminEndpoints.Map),
    ];
}
