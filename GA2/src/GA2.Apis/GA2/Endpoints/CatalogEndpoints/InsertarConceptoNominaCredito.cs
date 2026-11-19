using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]
    public class InsertarConceptoNomina : BaseAsyncEndpoint
        .WithRequest<ConceptoNominaDto>
        .WithResponse<Response<ConceptoNominaDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public InsertarConceptoNomina(
            ICatalogosBusinessLogic catalogosBusinessLogic,
            IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/Catalogos/InsertarConceptoNomina")]
        [ProducesResponseType(typeof(Response<ConceptoNominaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar concepto nómina",
            Description = "Permite insertar un concepto de nómina",
            OperationId = "catalogos.InsertarConceptoNomina",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<ConceptoNominaDto>>> HandleAsync(
            ConceptoNominaDto request,
            CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnName = User.Claims.FirstOrDefault(c => c.Type == "USR_APELLIDO")?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "USR_ID")?.Value;

            var user = $"{name} {surnName}";

            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);

            try
            {
                request.CreadoPor = Guid.Parse(userId);
                request.FechaCreacion = DateTime.Now;

                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CATALOGO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar información del concepto de nómina",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"
                };

                var dataResponse = await _catalogosBusinessLogic.InsertarConceptoNomina(request);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);

                await _auditoriaBusinessLogic.LogAuditAsync(
                    transation.Data.Id,
                    request,
                    null,
                    userId);

                return Ok(dataResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<ConceptoNominaDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = ex.Message
                    });
            }
        }
    }
}