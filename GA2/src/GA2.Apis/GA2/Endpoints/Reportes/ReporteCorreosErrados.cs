using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;
using GA2.Transversals.Commons;
using GA2.Domain.Entities.Reportes;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// Endpoint ReporteCorreosErrados genera el reporte en excel de los clientes en caja honor con correos errados
    /// </summary>
    [Authorize]
    public class ReporteCorreosErrados : BaseAsyncEndpoint
        .WithRequest<ParamCorreosErrados>
        .WithResponse<Response<byte[]>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteGeneralCarteraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCorreosErrados(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

       /// <summary>
       /// Metodo Generar reporte correos errados 
       /// </summary>
       /// <param name="param"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        [HttpPost("api/Reportes/ReporteCorreosErrados")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteGeneralCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Correos Errados",
            Description = "Generar Reporte Correos Errados",
            OperationId = "Reportes.ReporteCorreosErrados",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<byte[]>>> HandleAsync(ParamCorreosErrados param, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteCorreosErrados(param);
        }
    }
}
