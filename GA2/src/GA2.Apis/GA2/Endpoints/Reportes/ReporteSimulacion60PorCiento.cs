using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.Reportes
{
    //[Authorize]
    public class ReporteSimulacion60PorCiento : BaseAsyncEndpoint

        .WithRequest<ReporteSimulacion60PorCientoDto>
        .WithResponse<Response<SimulacionCreditoArchivo>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSimulacion60PorCiento(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteSimulacion60PorCiento")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Simulacion Credito",
            Description = "Generar Reporte Simulacion Credito",
            OperationId = "Reportes.ReporteSimulacion60PorCiento",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<SimulacionCreditoArchivo>>> HandleAsync(ReporteSimulacion60PorCientoDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSimulacion60PorCiento(request);

        }
    }

}
