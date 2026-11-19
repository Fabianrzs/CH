using System.Text.Json.Serialization;

namespace MockIntegrations.Api.Providers.KtrionApi.Models;

// La pasarela recibe los proxies WCF de WorkManager serializados con JsonConvert por defecto
// (nombres de los [DataMember], PascalCase salvo 'request', 'nextStepUser' y 'workflowId') y responde
// Response<T> de GA2. Los nombres se fijan con [JsonPropertyName] para que el JSON y el esquema
// OpenAPI coincidan con lo que GA2 envía y espera, sin depender de la política de nombres.

/// <summary><c>Response&lt;T&gt;</c> de GA2 (<c>GA2.Transversals/Commons/Response/Response.cs</c>).</summary>
public sealed class GatewayResponse<T>
{
    [JsonPropertyName("IsSuccess")] public bool IsSuccess { get; set; } = true;

    [JsonPropertyName("ReturnMessage")] public string? ReturnMessage { get; set; }

    [JsonPropertyName("Data")] public T? Data { get; set; }
}

/// <summary><c>LoginIntegracionesDto</c>: credenciales de la aplicación (<c>AppMain:*</c>).</summary>
public sealed class LoginIntegracionesRequest
{
    /// <summary>CorrelationId de la petición (igual a <c>UserId</c>).</summary>
    [JsonPropertyName("Id")] public Guid Id { get; set; }

    [JsonPropertyName("UserId")] public Guid UserId { get; set; }

    /// <summary><c>AppMain:ApplicationName</c>.</summary>
    [JsonPropertyName("NombreAplicacion")] public string? NombreAplicacion { get; set; }

    /// <summary><c>AppMain:Password</c>. El mock no lo valida.</summary>
    [JsonPropertyName("Contrasena")] public string? Contrasena { get; set; }

    /// <summary><c>AppMain:ApplicationId</c>.</summary>
    [JsonPropertyName("AplicacionId")] public string? AplicacionId { get; set; }
}

public sealed class TokenData
{
    /// <summary>Token que GA2 envía después como <c>Authorization: Bearer</c>.</summary>
    [JsonPropertyName("Token")] public string? Token { get; set; }
}

public sealed class HeaderDto
{
    [JsonPropertyName("Token")] public string? Token { get; set; }

    [JsonPropertyName("User")] public string? User { get; set; }
}

public sealed class DictionaryDto
{
    [JsonPropertyName("Field")] public string? Field { get; set; }

    [JsonPropertyName("Key")] public string? Key { get; set; }

    [JsonPropertyName("Value")] public string? Value { get; set; }
}

public sealed class FileDto
{
    [JsonPropertyName("Base64String")] public string? Base64String { get; set; }

    [JsonPropertyName("Description")] public string? Description { get; set; }

    [JsonPropertyName("DirectoryCode")] public string? DirectoryCode { get; set; }

    [JsonPropertyName("DocumentTypeCode")] public string? DocumentTypeCode { get; set; }

    [JsonPropertyName("Ext")] public string? Ext { get; set; }

    [JsonPropertyName("GUID")] public string? Guid { get; set; }
}

public sealed class FilterParametersDto
{
    [JsonPropertyName("Field")] public string? Field { get; set; }

    [JsonPropertyName("Operator")] public string? Operator { get; set; }

    [JsonPropertyName("Value")] public string? Value { get; set; }
}

public sealed class InsertRequestDto
{
    [JsonPropertyName("CheckWorkflow")] public bool CheckWorkflow { get; set; }

    [JsonPropertyName("Data")] public List<DictionaryDto>? Data { get; set; }

    [JsonPropertyName("Files")] public List<FileDto>? Files { get; set; }

    /// <summary>Código del formulario (p. ej. <c>83</c>); prefijo del radicado devuelto.</summary>
    [JsonPropertyName("FormCode")] public string? FormCode { get; set; }

    [JsonPropertyName("Header")] public HeaderDto? Header { get; set; }

    [JsonPropertyName("OfficeCode")] public string? OfficeCode { get; set; }

    [JsonPropertyName("OperationUser")] public string? OperationUser { get; set; }

    [JsonPropertyName("ProcessId")] public int? ProcessId { get; set; }

    [JsonPropertyName("nextStepUser")] public string? NextStepUser { get; set; }
}

public sealed class UpdateRequestDto
{
    [JsonPropertyName("Data")] public List<DictionaryDto>? Data { get; set; }

    [JsonPropertyName("FilterParameters")] public FilterParametersDto? FilterParameters { get; set; }

    [JsonPropertyName("FormCode")] public string? FormCode { get; set; }

    [JsonPropertyName("Header")] public HeaderDto? Header { get; set; }

    [JsonPropertyName("OperationUser")] public string? OperationUser { get; set; }
}

public sealed class WorkflowAttachFilesRequestDto
{
    [JsonPropertyName("Files")] public List<FileDto>? Files { get; set; }

    [JsonPropertyName("Header")] public HeaderDto? Header { get; set; }

    [JsonPropertyName("OperationUser")] public string? OperationUser { get; set; }

    [JsonPropertyName("WorkflowId")] public long WorkflowId { get; set; }
}

/// <summary>Proxy WCF <c>Form_InsertRequest</c> serializado (campo público <c>request</c>).</summary>
public sealed class FormInsertGatewayRequest
{
    [JsonPropertyName("request")] public InsertRequestDto? Request { get; set; }
}

public sealed class FormUpdateGatewayRequest
{
    [JsonPropertyName("request")] public UpdateRequestDto? Request { get; set; }
}

public sealed class WorkflowAttachFilesGatewayRequest
{
    [JsonPropertyName("request")] public WorkflowAttachFilesRequestDto? Request { get; set; }
}

/// <summary><c>InsertResponseDto</c> de WorkManager (orden y nombres de los DataMember).</summary>
public sealed class InsertResponseDto
{
    /// <summary>Número de radicado <c>{FormCode}-{consecutivo}</c>; vacío → GA2 guarda "RADICADO NO DISPONIBLE".</summary>
    [JsonPropertyName("BarCode")] public string? BarCode { get; set; }

    [JsonPropertyName("FileCodes")] public List<string>? FileCodes { get; set; }

    [JsonPropertyName("Message")] public string? Message { get; set; }

    [JsonPropertyName("RecordId")] public int RecordId { get; set; }

    [JsonPropertyName("Success")] public bool Success { get; set; }

    [JsonPropertyName("workflowId")] public int WorkflowId { get; set; }
}

public sealed class ResponseDto
{
    [JsonPropertyName("Data")] public List<DictionaryDto>? Data { get; set; }

    [JsonPropertyName("Message")] public string? Message { get; set; }

    [JsonPropertyName("Rows")] public int Rows { get; set; }

    [JsonPropertyName("Success")] public bool Success { get; set; }
}

public sealed class FormInsertGatewayResult
{
    [JsonPropertyName("Form_InsertResult")] public InsertResponseDto? FormInsertResult { get; set; }
}

public sealed class FormUpdateGatewayResult
{
    [JsonPropertyName("Form_UpdateResult")] public ResponseDto? FormUpdateResult { get; set; }
}

public sealed class WorkflowAttachFilesGatewayResult
{
    [JsonPropertyName("Workflow_AttachFilesResult")] public InsertResponseDto? WorkflowAttachFilesResult { get; set; }
}

/// <summary><c>HaberesRequestDto</c> de GA2.</summary>
public sealed class HaberesGatewayRequest
{
    [JsonPropertyName("Reporte")] public string? Reporte { get; set; }

    [JsonPropertyName("ClienteIdentificacion")] public string? ClienteIdentificacion { get; set; }

    [JsonPropertyName("CuentaId")] public int CuentaId { get; set; }

    [JsonPropertyName("Mes")] public string? Mes { get; set; }

    [JsonPropertyName("Anio")] public string? Anio { get; set; }
}
