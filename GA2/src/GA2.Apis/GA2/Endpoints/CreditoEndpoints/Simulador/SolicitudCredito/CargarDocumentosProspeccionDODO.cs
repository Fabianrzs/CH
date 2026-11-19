using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Endpoint para cargar documentos de prospección
    /// </summary>
    /// <author>Alejandro Lara</author>
    public class CargarDocumentosProspeccionDODO
        : BaseAsyncEndpoint
            .WithRequest<RequestDocumentoSimDto>
            .WithResponse<Response<ResponseDocumentoSimDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CargarDocumentosProspeccionDODO(
            ICreditoBusinessLogic creditoBusinessLogic,
            IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("/api/prospeccion/CargarDocumentosProspeccionDODO")]
        [ProducesResponseType(typeof(Response<ResponseDocumentoSimDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            "Cargar Documentos Prospección DODO",
            Description = "Carga documentos generados en simulación hacia WorkManager y BD",
            OperationId = "prospeccion.CargarDocumentosProspeccionDODO",
            Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<ResponseDocumentoSimDto>>> HandleAsync(
            RequestDocumentoSimDto request,
            CancellationToken cancellationToken = default)
        {
            var firstName = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (firstName == null)
            {
                // Usuario sin sesión cargada desde FE
                var userId = "83E91A3C-02A3-4D49-995E-87154620F98D";
                var userName = "AFILIADO";
                var ip = _auditoriaBusinessLogic.Ip();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var browserData = _auditoriaBusinessLogic.ParseUserAgent(userAgent);

                var auditRequest = new AuditoriaDto
                {
                    Modulo = "CREDITO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = browserData.OperatingSystem,
                    Descripcion = "Carga de documentos de prospección",
                    Navegador = browserData.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = userName,
                    TipoAccion = "GUARDAR"
                };

                var response = await _creditoBusinessLogic.CargarDocumentosProspeccion(request);
                var audit = await _auditoriaBusinessLogic.InsertarLogAuditoria(auditRequest);
                var requestRespons = response.Data;
                await _auditoriaBusinessLogic.LogAuditAsync<RequestDocumentoSimDto>(audit.Data.Id, request, null, userId);

                return response;
            }
            else
            {
                // Usuario autenticado normalmente
                var lastName = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault()?.Value;
                var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault()?.Value;
                var fullName = $"{firstName} {lastName}";
                var ip = _auditoriaBusinessLogic.Ip();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var browserData = _auditoriaBusinessLogic.ParseUserAgent(userAgent);

                var auditRequest = new AuditoriaDto
                {
                    Modulo = "CREDITO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = browserData.OperatingSystem,
                    Descripcion = "Carga de documentos de prospección",
                    Navegador = browserData.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = fullName,
                    TipoAccion = "GUARDAR"
                };

                var response = await _creditoBusinessLogic.CargarDocumentosProspeccion(request);
                var audit = await _auditoriaBusinessLogic.InsertarLogAuditoria(auditRequest);
                await _auditoriaBusinessLogic.LogAuditAsync(audit.Data.Id, request, null, userId);

                return response;
            }
        }
    }
}
