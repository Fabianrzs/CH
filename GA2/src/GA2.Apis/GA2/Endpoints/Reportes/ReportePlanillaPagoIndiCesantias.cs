using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Solicitud;
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
    public class ReportePlanillaPagoIndiCesantias : BaseAsyncEndpoint

        .WithRequest<ConsultarSolicitudDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePlanillaPagoIndiCesantias(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportePlanillaPagoIndiCesantias")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Planilla Pago individuales Cesantias",
            Description = "Generar Reporte Planilla Pago individuales Cesantias",
            OperationId = "Reportes.ReportePlanillaPagoIndiCesantias",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ConsultarSolicitudDto data, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReportePlanillaPagoIndiCesantias(data);

        }
    }

}
