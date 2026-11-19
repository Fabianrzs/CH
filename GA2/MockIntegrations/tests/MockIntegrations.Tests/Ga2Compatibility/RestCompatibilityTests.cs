using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Models;
using GA2.Transversals.Commons;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Providers.Dodo;
using MockIntegrations.Tests.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using DodoField = GA2.Domain.Core.Dodo.Field.Models;
using DodoFile = GA2.Domain.Core.Dodo.File.Models;
using DodoForms = GA2.Domain.Core.Dodo.Forms.Models;
using DodoLists = GA2.Domain.Core.Dodo.Lists.Models;
using DodoNotification = GA2.Domain.Core.Dodo.Notification.Models;
using DodoOffices = GA2.Domain.Core.Dodo.Offices.Models;
using DodoToken = GA2.Domain.Core.Dodo.Token.Models;
using DodoUser = GA2.Domain.Core.Dodo.User.Models;
using DodoWorkflow = GA2.Domain.Core.Dodo.Workflow.Models;
using Wm = GA2.Domain.Core.ReferenciasSvc.WorkManager;

namespace MockIntegrations.Tests.Ga2Compatibility;

/// <summary>
/// E8–E10: los requests se serializan y las respuestas se deserializan con los DTO y las opciones de GA2
/// (<c>CommonIntegracionCliente</c> para TusDatos/Dodo; <c>ProviderRequestFactory</c> + Newtonsoft para la pasarela).
/// </summary>
[Collection(MockServerCollection.Name)]
public sealed class RestCompatibilityTests(MockServer server) : IAsyncLifetime
{
    // CommonIntegracionCliente: request camelCase sin nulos; response sin distinguir mayúsculas.
    private static readonly JsonSerializerOptions Ga2RequestJson = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    private static readonly JsonSerializerOptions Ga2ResponseJson = new() { PropertyNameCaseInsensitive = true };

    // ProviderRequestFactory._serializerSettings.
    private static readonly JsonSerializerSettings Ga2GatewaySettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        NullValueHandling = NullValueHandling.Ignore,
    };

    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ─── E8 TusDatos ────────────────────────────────────────────────────────

    [Fact]
    public async Task TusDatos_job_goes_from_pending_to_finished()
    {
        using var client = TusDatosClient();

        var started = await GetAsync<DatosClienteResponse>(client, "api/v1/tusdatos/customer/1020304050/document-type/CC/check-history/true");
        Assert.False(started.IsFromHistory);
        Assert.NotNull(started.JobId);

        var pending = await GetAsync<DatosClienteJobResponse>(client, $"api/v1/tusdatos/jobid/{started.JobId}/customer/1020304050");
        var finished = await GetAsync<DatosClienteJobResponse>(client, $"api/v1/tusdatos/jobid/{started.JobId}/customer/1020304050");

        Assert.Equal("pendiente", pending.StatusJobTusDatos);
        Assert.Null(pending.HistoryDataCustomer);
        Assert.Equal("finalizado", finished.StatusJobTusDatos);
        Assert.False(finished.HistoryDataCustomer!.IsPEPS);
    }

    [Fact]
    public async Task TusDatos_history_pep_and_vigia_scenarios()
    {
        using var client = TusDatosClient();

        var history = await GetAsync<DatosClienteResponse>(client, "api/v1/tusdatos/customer/1000000100/document-type/CC/check-history/true");
        Assert.True(history.IsFromHistory);
        Assert.Null(history.JobId);

        var pep = await GetAsync<DatosClienteResponse>(client, "api/v1/tusdatos/customer/1000000666/document-type/CC/check-history/false");
        await GetAsync<DatosClienteJobResponse>(client, $"api/v1/tusdatos/jobid/{pep.JobId}/customer/1000000666");
        var pepResult = await GetAsync<DatosClienteJobResponse>(client, $"api/v1/tusdatos/jobid/{pep.JobId}/customer/1000000666");
        Assert.True(pepResult.HistoryDataCustomer!.IsPEPS);

        // GA2 reemplaza "api/v1/tusdatos" por "api/v1" para Vigía.
        var vigia = await GetAsync<VigiaResponse>(client, "api/v1/vigia/customer/1020304050/document-type/CC/CheckRiskLAFT");
        Assert.Equal("1020304050", vigia.Identification);
        Assert.Equal("CC", vigia.DocumentType);
        Assert.False(string.IsNullOrEmpty(vigia.RiskLAFT));
    }

    [Fact]
    public async Task TusDatos_status_codes_match_what_CommonIntegracionCliente_handles()
    {
        using var client = TusDatosClient();
        using var notFound = await client.GetAsync("api/v1/tusdatos/customer/1000000404/document-type/CC/check-history/true");
        Assert.Equal(HttpStatusCode.NotFound, notFound.StatusCode); // GA2: éxito con Data = null

        using var anonymous = new HttpClient { BaseAddress = new Uri(server.BaseUri, "kiosko-dev/") };
        using var unauthorized = await anonymous.GetAsync("api/v1/tusdatos/customer/1/document-type/CC/check-history/true");
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode); // GA2: IsSuccess=false con el cuerpo
        Assert.False(string.IsNullOrWhiteSpace(await unauthorized.Content.ReadAsStringAsync()));

        using var unknownJob = await client.GetAsync("api/v1/tusdatos/jobid/no-existe/customer/1");
        Assert.Equal(HttpStatusCode.NotFound, unknownJob.StatusCode);
    }

    // ─── E9 Dodo REST ───────────────────────────────────────────────────────

    public static TheoryData<string, Type, Type> DodoOperations() => new()
    {
        { "api/Field/Field_GetValues", typeof(DodoField.FieldGetValuesRequest), typeof(DodoField.FieldGetValuesResponse) },
        { "api/Lists/List_GetItems", typeof(DodoLists.ListGetItemsRequest), typeof(DodoLists.ListGetItemsResponse) },
        { "api/File/File_CopyToEP", typeof(DodoFile.FileCopyToEPRequest), typeof(DodoFile.FileSimpleResponse) },
        { "api/File/File_CopyToFather", typeof(DodoFile.FileCopyToFatherRequest), typeof(DodoFile.FileSimpleResponse) },
        { "api/File/File_Delete", typeof(DodoFile.FileDeleteRequest), typeof(DodoFile.FileSimpleResponse) },
        { "api/Forms/Form_GetParameters", typeof(DodoForms.FormGetParametersRequest), typeof(DodoForms.FormGetParametersResponse) },
        { "api/Forms/Form_Insert", typeof(DodoForms.FormInsertRequest), typeof(DodoFileOperationResponse) },
        { "api/Forms/Form_Update", typeof(DodoForms.FormUpdateRequest), typeof(DodoForms.FormUpdateResponse) },
        { "api/Forms/Form_Delete", typeof(DodoForms.FormDeleteRequest), typeof(DodoForms.FormDeleteResponse) },
        { "api/Forms/Form_GetData", typeof(DodoForms.FormGetDataRequest), typeof(DodoForms.FormGetDataResponse) },
        { "api/Forms/Form_AttachFiles", typeof(DodoForms.FormAttachFilesRequest), typeof(DodoFileOperationResponse) },
        { "api/Forms/Form_GetFileCodes", typeof(DodoForms.FormGetFileCodesRequest), typeof(DodoForms.FormGetFileCodesResponse) },
        { "api/Forms/Form_GetFilesDirectories", typeof(DodoForms.FormGetFilesDirectoriesRequest), typeof(DodoForms.FormGetFilesDirectoriesResponse) },
        { "api/Forms/Form_GetBase64StringFile", typeof(DodoForms.FormGetBase64StringFileRequest), typeof(DodoForms.FormGetBase64StringFileResponse) },
        { "api/Notification/SendEmail", typeof(DodoNotification.SendEmailRequest), typeof(DodoNotification.SendEmailResponse) },
        { "api/Offices/GetOffices", typeof(DodoOffices.GetOfficesRequest), typeof(DodoOffices.GetOfficesResponse) },
        { "api/Token/GetToken", typeof(DodoToken.GetTokenRequest), typeof(DodoToken.GetTokenResponse) },
        { "api/User/User_Info", typeof(DodoUser.UserInfoRequest), typeof(DodoUser.UserInfoResponse) },
        { "api/Workflow/Workflow_Start", typeof(DodoWorkflow.WorkflowStartRequest), typeof(DodoWorkflow.WorkflowStartResponse) },
        { "api/Workflow/Workflow_RunTask", typeof(DodoWorkflow.WorkflowRunTaskRequest), typeof(DodoWorkflow.WorkflowSimpleResponse) },
        { "api/Workflow/Workflow_ReturnTask", typeof(DodoWorkflow.WorkflowReturnTaskRequest), typeof(DodoWorkflow.WorkflowSimpleResponse) },
        { "api/Workflow/Workflow_AbortProcess", typeof(DodoWorkflow.WorkflowAbortProcessRequest), typeof(DodoWorkflow.WorkflowSimpleResponse) },
        { "api/Workflow/Workflow_AttachFiles", typeof(DodoWorkflow.WorkflowAttachFilesRequest), typeof(DodoFileOperationResponse) },
        { "api/Workflow/Workflow_GetCases", typeof(DodoWorkflow.WorkflowGetCasesRequest), typeof(DodoWorkflow.WorkflowGetCasesResponse) },
        { "api/Workflow/Workflow_GetTasks", typeof(DodoWorkflow.WorkflowGetTasksRequest), typeof(DodoWorkflow.WorkflowGetTasksResponse) },
        { "api/Workflow/Workflow_GetFileCodes", typeof(DodoWorkflow.WorkflowGetFileCodesRequest), typeof(DodoWorkflow.WorkflowGetFileCodesResponse) },
        { "api/Workflow/Workflow_StepsForReturn", typeof(DodoWorkflow.WorkflowStepsForReturnRequest), typeof(DodoWorkflow.WorkflowStepsForReturnResponse) },
        { "api/Workflow/Process_Parameters", typeof(DodoWorkflow.ProcessParametersRequest), typeof(DodoWorkflow.ProcessParametersResponse) },
    };

    [Theory]
    [MemberData(nameof(DodoOperations))]
    public async Task Dodo_operation_accepts_the_GA2_request_and_returns_the_GA2_response(string route, Type requestType, Type responseType)
    {
        // Request tal como lo serializa GA2 (los defaults de DodoFileItem siempre se envían).
        var request = Activator.CreateInstance(requestType)!;
        if (requestType.GetProperty("Header") is { } header)
        {
            header.SetValue(request, new DodoHeader { User = "USR", Token = "TKN" });
        }

        using var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request, requestType, Ga2RequestJson), Encoding.UTF8, "application/json");
        using var response = await server.Http.PostAsync(DodoMock.Prefix.TrimStart('/') + route, content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = System.Text.Json.JsonSerializer.Deserialize(await response.Content.ReadAsStringAsync(), responseType, Ga2ResponseJson)!;
        Assert.True((bool)responseType.GetProperty("Success")!.GetValue(body)!);
    }

    [Fact]
    public async Task Dodo_form_insert_returns_a_barcode_and_failure_scenario_sets_success_false()
    {
        var request = new DodoForms.FormInsertRequest
        {
            FormCode = "02",
            OperationUser = "mock",
            OfficeCode = "001",
            Files = [new DodoFileItem { Ext = "pdf", Description = "Soporte", Base64String = "JVBERi0=" }],
        };

        var created = await PostDodoAsync<DodoFileOperationResponse>("api/Forms/Form_Insert", request);
        Assert.StartsWith("02-", created.BarCode);
        Assert.Single(created.FileCodes);

        await server.SetScenarioAsync(DodoMock.Provider, "Form_Insert", "fallido");
        var failed = await PostDodoAsync<DodoFileOperationResponse>("api/Forms/Form_Insert", request);
        Assert.False(failed.Success);
        Assert.False(string.IsNullOrEmpty(failed.Message));
    }

    [Fact]
    public async Task Dodo_version_is_a_json_string()
    {
        var version = await server.Http.GetFromJsonAsync<string>("dodo/api/version", Ga2ResponseJson);
        Assert.False(string.IsNullOrEmpty(version));
    }

    // ─── E10 Pasarela ktrionApi ─────────────────────────────────────────────

    [Fact]
    public async Task Gateway_token_then_forminsert_as_ProviderRequestFactory_does()
    {
        var token = await GatewayTokenAsync();

        // GA2 serializa el proxy WCF con JsonConvert por defecto y deserializa Response<Form_InsertResponse>.
        var request = new Wm.Form_InsertRequest(new Wm.InsertRequestDto { FormCode = "83", CheckWorkflow = true, Header = new Wm.HeaderDto { User = "u", Token = "t" }, Data = [], Files = [] });
        var response = await GatewayPostAsync<Response<Wm.Form_InsertResponse>>("api/forminsert", request, token);

        Assert.True(response.IsSuccess);
        Assert.Equal("83", response.Data.Form_InsertResult.BarCode.Split("-")[0]);
        Assert.True(response.Data.Form_InsertResult.Success);
    }

    [Fact]
    public async Task Gateway_formUpdate_workflowattachfiles_and_haberes()
    {
        var update = await GatewayPostAsync<Response<Wm.Form_UpdateResponse>>("api/formUpdate",
            new Wm.Form_UpdateRequest(new Wm.UpdateRequestDto { FormCode = "83", Data = [new Wm.DictionaryDto { Field = "C0098", Value = "X" }] }), await GatewayTokenAsync());
        Assert.True(update.Data.Form_UpdateResult.Success);
        Assert.Equal("C0098", update.Data.Form_UpdateResult.Data[0].Field);

        var attach = await GatewayPostAsync<Response<Wm.Workflow_AttachFilesResponse>>("api/workflowattachfiles",
            new Wm.Workflow_AttachFilesRequest(new Wm.WorkflowAttachFilesRequestDto { WorkflowId = 9981, Files = [new Wm.FileDto { Ext = "pdf" }] }), await GatewayTokenAsync());
        Assert.True(attach.Data.Workflow_AttachFilesResult.Success);
        Assert.Equal(9981, attach.Data.Workflow_AttachFilesResult.workflowId);
        Assert.Single(attach.Data.Workflow_AttachFilesResult.FileCodes);

        var haberes = await GatewayPostAsync<Response<string>>("api/generarreportehaberes",
            new { Reporte = "CERTIFICADO", ClienteIdentificacion = "1020304050", CuentaId = 1, Mes = (string?)null, Anio = (string?)null }, await GatewayTokenAsync());
        Assert.StartsWith("%PDF", Encoding.ASCII.GetString(Convert.FromBase64String(haberes.Data)));
    }

    [Fact]
    public async Task Gateway_rejects_calls_without_an_issued_token()
    {
        using var content = new StringContent(JsonConvert.SerializeObject(new Wm.Form_InsertRequest(new Wm.InsertRequestDto())), Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "REST/KriterionDev/ktrionApi/api/forminsert") { Content = content };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "no-emitido");

        using var response = await server.Http.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // GA2: throw new Exception(body) → 409
    }

    [Fact]
    public async Task Gateway_unused_routes_answer_501()
    {
        var spec = server.GetService<MockCatalog>().Operations.Where(o => o.Provider == "KtrionApi" && o.Usage == Ga2Usage.NotUsed).ToList();
        Assert.Equal(6, spec.Count);
        using var response = await server.Http.PostAsync(spec[0].Route.TrimStart('/'), new StringContent("{}", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    private HttpClient TusDatosClient()
    {
        // Mismo registro que HttpClientExtensions.AddTusDatosIntegration.
        var client = new HttpClient { BaseAddress = new Uri(server.BaseUri, "kiosko-dev/"), Timeout = TimeSpan.FromSeconds(15) };
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("x-api-key", "clave-de-prueba");
        return client;
    }

    private static async Task<T> GetAsync<T>(HttpClient client, string path)
    {
        using var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return System.Text.Json.JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), Ga2ResponseJson)!;
    }

    private async Task<T> PostDodoAsync<T>(string route, object request)
    {
        using var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request, request.GetType(), Ga2RequestJson), Encoding.UTF8, "application/json");
        using var response = await server.Http.PostAsync("dodo/" + route, content);
        response.EnsureSuccessStatusCode();
        return System.Text.Json.JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), Ga2ResponseJson)!;
    }

    private async Task<string> GatewayTokenAsync()
    {
        var login = new { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), NombreAplicacion = "IntegracionesKriterion", Contrasena = "<AppMain:Password>", AplicacionId = "<AppMain:ApplicationId>" };
        var token = await GatewayPostAsync<Response<TokenData>>("api/autenticacion", login, null);
        return token.Data.Token;
    }

    private async Task<T> GatewayPostAsync<T>(string route, object body, string? token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "REST/KriterionDev/ktrionApi/" + route)
        {
            Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
        };
        if (token is not null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        using var response = await server.Http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), Ga2GatewaySettings)!;
    }

    /// <summary>Forma de <c>TokenDto</c> de GA2 (GA2.Application/Dto/Token/TokenDto.cs).</summary>
    private sealed class TokenData
    {
        public string Token { get; set; } = "";
    }
}
