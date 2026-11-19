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
    public class ActualizarSexo:BaseAsyncEndpoint.WithRequest<SexoDto>.WithResponse<Response<SexoDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Obtener Tipo De Sexo
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <date>18/05/2021</date>
        public ActualizarSexo(ICatalogosBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("api/Catalogos/ActualizarSexo")]
        [ProducesResponseType(typeof(Response<SexoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza sexos del catalogo",
           Description = "Actualiza sexos del catalogo",
           OperationId = "catalogos.ActualizarSexo",
           Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<SexoDto>>> HandleAsync(SexoDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "Actualizacion de tipo de sexo.",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var tiposSexos = await _catalogsBusinessLogic.ObtenerTipoDeSexo();
                var sexo = tiposSexos.Data.Where(x => x.Id == request.Id).FirstOrDefault();
                var response = await _catalogsBusinessLogic.ActualizarSexo(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, sexo, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _catalogsBusinessLogic.ActualizarSexo(request);
        }
    }
}
