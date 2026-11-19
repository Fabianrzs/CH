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
    public class ReporteSimulacionCredito : BaseAsyncEndpoint

        .WithRequest<List<SimulacionDto>>
        .WithResponse<Response<SimulacionCreditoArchivo>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSimulacionCredito(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteSimulacionCredito")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Simulacion Credito",
            Description = "Generar Reporte Simulacion Credito",
            OperationId = "Reportes.ReporteSimulacionCredito",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<SimulacionCreditoArchivo>>> HandleAsync(List<SimulacionDto> request,CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSimulacionCredito(request,1);

        }
    }

}
