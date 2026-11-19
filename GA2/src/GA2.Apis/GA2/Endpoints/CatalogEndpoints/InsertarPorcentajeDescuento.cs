using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]
    public class InsertarPorcentajeDescuento : BaseAsyncEndpoint
        .WithRequest<PorcentajeDescuentoDto>
        .WithResponse<Response<PorcentajeDescuentoDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Insertar Porcentaje Descuento
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public InsertarPorcentajeDescuento(ICatalogosBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Inserta un porcentaje de descuento en el catálogo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/catalogos/InsertarPorcentajesDescuento")]
        [ProducesResponseType(typeof(Response<PorcentajeDescuentoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Inserta un porcentaje de descuento en el catálogo",
            Description = "Inserta un porcentaje de descuento en el catálogo",
            OperationId = "catalogos.InsertarPorcentajesDescuento",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<PorcentajeDescuentoDto>>> HandleAsync(PorcentajeDescuentoDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CATALOGO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion del porcentaje de descuento",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _catalogsBusinessLogic.InsertarPorcentajeDescuento(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}
