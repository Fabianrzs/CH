using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    [Authorize]
    public class GenerarReporteAuditoriaCronograma :
        BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<GeneracionArchivosResponse>>      
    {
        private readonly IAuditoriaNovedadesBussinessLogic _auditoriaNovedadesBussinessLogic;
        public GenerarReporteAuditoriaCronograma(IAuditoriaNovedadesBussinessLogic auditoriaNovedadesBussinessLogic)
        {
            _auditoriaNovedadesBussinessLogic = auditoriaNovedadesBussinessLogic;
        }

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/AuditoriaNovedades/ObtenerReporteNovedades")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<GeneracionArchivosResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Reporte Novedades",
           Description = "Obtener Reporte Novedades",
           OperationId = "ReporteAuditoriaNovedades.ConsultarAuditoriaNovedades",
           Tags = new[] { "ReporteAuditoriaNovedadesEndpoint" })]
        public async override Task<ActionResult<Response<GeneracionArchivosResponse>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _auditoriaNovedadesBussinessLogic.GenerarExcelNovedaes(1);            
        }
    }
}
