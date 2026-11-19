using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Afiliaciones
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class GenerarReportesAfiliacion : BaseAsyncEndpoint
        .WithRequest<ParametrosReportesAfiliacionesDto>
        .WithResponse<Response<IEnumerable<OficiosAfiliacionDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public GenerarReportesAfiliacion(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// ReporteCarteraInformacionCliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/GenerarReportesAfiliacion")]
        [SwaggerOperation(
           Summary = "Generar Reportes Afiliacion",
           Description = "Generar Reportes Afiliacion",
           OperationId = "afiliaciones.GenerarReportesAfiliacion",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<OficiosAfiliacionDto>>>> HandleAsync([FromQuery] ParametrosReportesAfiliacionesDto request, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.GenerarReportesAfiliacion(request);
        }
    }
}
