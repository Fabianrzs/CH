using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GA2.Domain.Entities.Integraciones;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;

namespace GA2.Domain.Core.ReferenciasSvc.Common
{
    public abstract class CommonIntegracionCliente
    {
        private readonly HttpClient _httpClient;
        private readonly IEstadoIntegracionProvider _integracionProvider;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ILogger<CommonIntegracionCliente> _logger;

        protected abstract ReferenciaSvc ReferenciaServicio { get; }

        protected CommonIntegracionCliente(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<CommonIntegracionCliente> logger)
        {
            _httpClient = httpClient;
            _integracionProvider = integracionProvider;
            _auditoriaRepository = auditoriaRepository;
            _logger = logger;
        }


        protected async Task<Response<string>> GetXmlAsync(
                string endpoint,
                CancellationToken cancellationToken = default)
        {
            await ValidateIntegrationStatus();

            var response = await _httpClient.GetAsync(endpoint, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = responseContent,
                    Data = null
                };
            }

            return new Response<string>
            {
                IsSuccess = true,
                ReturnMessage = "Consulta exitosa",
                Data = responseContent
            };
        }

        #region GET

        protected async Task<Response<T>> GetAsync<T>(
            string endpoint,
            CancellationToken cancellationToken = default)
        {
            await ValidateIntegrationStatus();

            var auditoria = CrearAuditoria(endpoint, "GET", null);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation(
                    "Consumiendo integración {Servicio} GET {Endpoint}",
                    ReferenciaServicio,
                    endpoint);

                var response = await _httpClient.GetAsync(
                    endpoint,
                    cancellationToken);

                auditoria.STATUSCODE = (int)response.StatusCode;

                var responseContent = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                auditoria.RESPONSE = responseContent;
                auditoria.SUCCESS = response.IsSuccessStatusCode;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new Response<T>
                    {
                        IsSuccess = true,
                        ReturnMessage = "Resource not found",
                        Data = default
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    auditoria.ERROR = responseContent;

                    return new Response<T>
                    {
                        IsSuccess = false,
                        ReturnMessage = responseContent,
                        Data = default
                    };
                }

                var data = JsonSerializer.Deserialize<T>(
                    responseContent,
                    JsonOptions());

                return new Response<T>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta exitosa",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                auditoria.SUCCESS = false;
                auditoria.ERROR = ex.ToString();

                _logger.LogError(
                    ex,
                    "Error consumiendo integración {Servicio}",
                    ReferenciaServicio);

                return new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message,
                    Data = default
                };
            }
            finally
            {
                stopwatch.Stop();

                auditoria.FECHAFIN = DateTime.UtcNow;
                auditoria.DURACIONMS = stopwatch.ElapsedMilliseconds;

                await GuardarAuditoria(auditoria);
            }
        }

        #endregion

        #region POST

        protected async Task<Response<TResponse>> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            await ValidateIntegrationStatus();

            var auditoria = CrearAuditoria(
                endpoint,
                "POST",
                request);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var json = Serialize(request);

                var httpContent = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                _logger.LogInformation(
                    "Consumiendo integración {Servicio} POST {Endpoint}",
                    ReferenciaServicio,
                    endpoint);

                var response = await _httpClient.PostAsync(
                    endpoint,
                    httpContent,
                    cancellationToken);

                auditoria.STATUSCODE = (int)response.StatusCode;

                var responseContent = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                auditoria.RESPONSE = responseContent;
                auditoria.SUCCESS = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    auditoria.ERROR = responseContent;

                    return new Response<TResponse>
                    {
                        IsSuccess = false,
                        ReturnMessage = responseContent,
                        Data = default
                    };
                }

                var data = JsonSerializer.Deserialize<TResponse>(
                    responseContent,
                    JsonOptions());

                return new Response<TResponse>
                {
                    IsSuccess = true,
                    ReturnMessage = "Proceso exitoso",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                auditoria.SUCCESS = false;
                auditoria.ERROR = ex.ToString();

                _logger.LogError(
                    ex,
                    "Error consumiendo integración {Servicio}",
                    ReferenciaServicio);

                return new Response<TResponse>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message,
                    Data = default
                };
            }
            finally
            {
                stopwatch.Stop();

                auditoria.FECHAFIN = DateTime.UtcNow;
                auditoria.DURACIONMS = stopwatch.ElapsedMilliseconds;

                await GuardarAuditoria(auditoria);
            }
        }

        #endregion

        #region PUT

        protected async Task<Response<TResponse>> PutAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            await ValidateIntegrationStatus();

            var auditoria = CrearAuditoria(
                endpoint,
                "PUT",
                request);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var json = Serialize(request);

                var httpContent = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                _logger.LogInformation(
                    "Consumiendo integración {Servicio} PUT {Endpoint}",
                    ReferenciaServicio,
                    endpoint);

                var response = await _httpClient.PutAsync(
                    endpoint,
                    httpContent,
                    cancellationToken);

                auditoria.STATUSCODE = (int)response.StatusCode;

                var responseContent = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                auditoria.RESPONSE = responseContent;
                auditoria.SUCCESS = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    auditoria.ERROR = responseContent;

                    return new Response<TResponse>
                    {
                        IsSuccess = false,
                        ReturnMessage = responseContent,
                        Data = default
                    };
                }

                var data = JsonSerializer.Deserialize<TResponse>(
                    responseContent,
                    JsonOptions());

                return new Response<TResponse>
                {
                    IsSuccess = true,
                    ReturnMessage = "Actualización exitosa",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                auditoria.SUCCESS = false;
                auditoria.ERROR = ex.ToString();

                _logger.LogError(
                    ex,
                    "Error consumiendo integración {Servicio}",
                    ReferenciaServicio);

                return new Response<TResponse>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message,
                    Data = default
                };
            }
            finally
            {
                stopwatch.Stop();

                auditoria.FECHAFIN = DateTime.UtcNow;
                auditoria.DURACIONMS = stopwatch.ElapsedMilliseconds;

                await GuardarAuditoria(auditoria);
            }
        }

        #endregion

        #region DELETE

        protected async Task<Response<bool>> DeleteAsync(
            string endpoint,
            CancellationToken cancellationToken = default)
        {
            await ValidateIntegrationStatus();

            var auditoria = CrearAuditoria(
                endpoint,
                "DELETE",
                null);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation(
                    "Consumiendo integración {Servicio} DELETE {Endpoint}",
                    ReferenciaServicio,
                    endpoint);

                var response = await _httpClient.DeleteAsync(
                    endpoint,
                    cancellationToken);

                auditoria.STATUSCODE = (int)response.StatusCode;

                var responseContent = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                auditoria.RESPONSE = responseContent;
                auditoria.SUCCESS = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    auditoria.ERROR = responseContent;

                    return new Response<bool>
                    {
                        IsSuccess = false,
                        ReturnMessage = responseContent,
                        Data = false
                    };
                }

                return new Response<bool>
                {
                    IsSuccess = true,
                    ReturnMessage = "Eliminación exitosa",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                auditoria.SUCCESS = false;
                auditoria.ERROR = ex.ToString();

                _logger.LogError(
                    ex,
                    "Error consumiendo integración {Servicio}",
                    ReferenciaServicio);

                return new Response<bool>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message,
                    Data = false
                };
            }
            finally
            {
                stopwatch.Stop();

                auditoria.FECHAFIN = DateTime.UtcNow;
                auditoria.DURACIONMS = stopwatch.ElapsedMilliseconds;

                await GuardarAuditoria(auditoria);
            }
        }

        #endregion

        #region PRIVATE METHODS

        private async Task ValidateIntegrationStatus()
        {
            if (ReferenciaServicio.Equals(ReferenciaSvc.Internal))
            {
                return;
            }

            var esActiva = await _integracionProvider
                .IntegracionActiva(ReferenciaServicio.ToString());

            if (!esActiva)
            {
                throw new InvalidOperationException(
                    $"La integración [{ReferenciaServicio}] se encuentra inactiva o no configurada.");
            }
        }

        private ReportIntegracion CrearAuditoria(
            string endpoint,
            string metodoHttp,
            object? request)
        {
            return new ReportIntegracion
            {
                ID_INTEGRACIONES = Guid.NewGuid(),
                TIPOSERVICIO = ReferenciaServicio.ToString(),
                METODOHTTP = metodoHttp,
                ENDPOINT = endpoint,
                REQUEST = Serialize(request),
                FECHAINICIO = DateTime.UtcNow,
                SUCCESS = false
            };
        }

        private async Task GuardarAuditoria(
            ReportIntegracion auditoria)
        {
            try
            {
                await _auditoriaRepository
                    .InsertReportIntegracionesRest(auditoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error guardando auditoría integración {Servicio}",
                    auditoria.TIPOSERVICIO);
            }
        }

        private static string Serialize(object? data)
        {
            if (data == null)
                return string.Empty;

            return JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = false
                });
        }

        private static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        #endregion
    }
}