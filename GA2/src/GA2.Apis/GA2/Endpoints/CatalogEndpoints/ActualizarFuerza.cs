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
    public class ActualizarFuerza:BaseAsyncEndpoint.WithRequest<FuerzasDto>.WithResponse<Response<FuerzasDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public ActualizarFuerza(ICatalogosBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogosBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// Obtiene all fuerzas
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("api/Catalogos/ActualizarFuerza")]
        [ProducesResponseType(typeof(Response<FuerzasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza catalogo",
            Description = "Actualiza catalogo",
            OperationId = "catalogos.ActualizarFuerza",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<FuerzasDto>>> HandleAsync(FuerzasDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
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
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizacion de  fuerza",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var fuerzas = await _catalogosBusinessLogic.ObtenerFuerzas();
                var fuerza = fuerzas.Data.Where(x => x.Id == request.Id).FirstOrDefault();
                var response = await _catalogosBusinessLogic.ActualizarFuerza(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, fuerza, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _catalogosBusinessLogic.ActualizarFuerza(request);
        }
    }
         

}
